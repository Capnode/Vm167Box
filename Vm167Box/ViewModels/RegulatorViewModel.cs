using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Vm167Box.Models;
using Vm167Box.Services;

namespace Vm167Box.ViewModels
{
    public partial class RegulatorViewModel : ObservableObject
    {
        const double Period = IVm167Service.Period / 1000.0;

        private readonly ILogger<RegulatorViewModel> _logger;
        private readonly IVm167Service _vm167Service;
        private readonly ISettingsService _settingsService;
        private readonly AnalogChannel[] _inputs;
        private readonly AnalogChannel[] _outputs;
        private readonly SafetyViewModel[] _safety;
        private double _previousError;
        private double _integral;
        private bool _running;
        private bool _stopping;

        public RegulatorViewModel(ILogger<RegulatorViewModel> logger, IVm167Service vm167Service, ISettingsService settingsService)
        {
            _logger = logger;
            _vm167Service = vm167Service;
            _settingsService = settingsService;
            _inputs = new AnalogChannel[]
            {
                _settingsService.Analog1,
                _settingsService.Analog2,
                _settingsService.Analog3,
                _settingsService.Analog4,
                _settingsService.Analog5,
                _settingsService.Analog6
            };

            _outputs = new AnalogChannel[]
            {
                _settingsService.Pwm1,
                _settingsService.Pwm2
            };

            _safety = new SafetyViewModel[]
            {
                new SafetyViewModel { Channel = _settingsService.Analog1 },
                new SafetyViewModel { Channel = _settingsService.Analog2 },
                new SafetyViewModel { Channel = _settingsService.Analog3 },
                new SafetyViewModel { Channel = _settingsService.Analog4 },
                new SafetyViewModel { Channel = _settingsService.Analog5 },
                new SafetyViewModel { Channel = _settingsService.Analog6 },
                new SafetyViewModel { Channel = _settingsService.Pwm1 },
                new SafetyViewModel { Channel = _settingsService.Pwm2 }
            };

            _referenceSignal = _inputs[0];
            _feedbackSignal = _inputs[0];
            _controlSignal = _outputs[0];

            UpdateSettings();
            SetStates();

            _vm167Service.Tick += Loop;
            _settingsService.Update += UpdateSettings;
        }

        public IEnumerable<AnalogChannel> Inputs => _inputs;

        public IEnumerable<AnalogChannel> Outputs => _outputs;

        public IEnumerable<SafetyViewModel> Safety => _safety;

        [ObservableProperty]
        private bool _isOpen;

        private AnalogChannel _referenceSignal;
        public AnalogChannel ReferenceSignal
        {
            get => _referenceSignal;
            set
            {
                if (SetProperty(ref _referenceSignal, value))
                {
                    _settingsService.ReferenceIndex = Array.IndexOf(_inputs, value);
                }
            }
        }

        private AnalogChannel _feedbackSignal;
        public AnalogChannel FeedbackSignal
        {
            get => _feedbackSignal;
            set
            {
                if (SetProperty(ref _feedbackSignal, value))
                {
                    _settingsService.FeedbackIndex = Array.IndexOf(_inputs, value);
                }
            }
        }

        private AnalogChannel _controlSignal;
        public AnalogChannel ControlSignal
        {
            get => _controlSignal;
            set
            {
                if (SetProperty(ref _controlSignal, value))
                {
                    _settingsService.ControlIndex = Array.IndexOf(_outputs, value);
                }
            }
        }

        private double _regulatorKp = 1;
        public double RegulatorKp
        {
            get => _regulatorKp;
            set
            {
                if (SetProperty(ref _regulatorKp, value))
                {
                    _settingsService.RegulatorKp = value;
                }
            }
        }

        private double _regulatorKi = 0;
        public double RegulatorKi
        {
            get => _regulatorKi;
            set
            {
                if (SetProperty(ref _regulatorKi, value))
                {
                    _settingsService.RegulatorKi = value;
                }
            }
        }

        private double _regulatorKd = 0;
        public double RegulatorKd
        {
            get => _regulatorKd;
            set
            {
                if (SetProperty(ref _regulatorKd, value))
                {
                    _settingsService.RegulatorKd = value;
                }
            }
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(StartCommand))]
        private bool _canStart;

        [RelayCommand(CanExecute = nameof(CanStart))]
        public void Start()
        {
            _previousError = 0;
            _integral = 0;
            _running = true;
            SetStates();
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(StopCommand))]
        private bool _canStop;

        [RelayCommand(CanExecute = nameof(CanStop))]
        private void Stop()
        {
            _stopping = true;
            SetStates();
        }

        internal void Save()
        {
            string safety = string.Join("", _safety.Select(s => s.IsSafe ? '1' : '0'));
            _settingsService.Safety = safety;
        }

        private Task Loop()
        {
            IsOpen = true;
            if (!_running) return Task.CompletedTask;

            if (ReferenceSignal == null || FeedbackSignal == null || ControlSignal == null)
            {
                return Task.CompletedTask;
            }

            foreach (var safety in _safety)
            {
                if (!safety.IsSafe) continue;
                var channel = safety.Channel;
                if (channel == null) continue;
                if (channel.Value > channel.MaxValue)
                {
                    _stopping = true;
                    MainThread.BeginInvokeOnMainThread(SetStates);
                }
            }

            if (_stopping)
            {
                _stopping = false;
                _running = false;
                ControlSignal.Value = 0;
                MainThread.BeginInvokeOnMainThread(SetStates);
                return Task.CompletedTask;
            }

            // PID regulator logic here

            // Calculate the error
            double error = ReferenceSignal.Value - FeedbackSignal.Value;

            // Integral term
            _integral += error * Period;

            // Derivative term
            double derivative = (error - _previousError) / Period;

            // Calculate the control signal
            ControlSignal.Value = (RegulatorKp * error) + (RegulatorKi * _integral) + (RegulatorKd * derivative);

            // Save the current error for the next loop
            _previousError = error;

            return Task.CompletedTask;
        }

        private void UpdateSettings()
        {
            OnPropertyChanged(nameof(Inputs));
            OnPropertyChanged(nameof(Outputs));

            var referenceIndex = _settingsService.ReferenceIndex;
            referenceIndex = referenceIndex >= 0 && referenceIndex < _inputs.Length ? referenceIndex : 0;
            ReferenceSignal = _inputs[referenceIndex];
            var feedbackIndex = _settingsService.FeedbackIndex;
            feedbackIndex = feedbackIndex >= 0 && feedbackIndex < _inputs.Length ? feedbackIndex : 0;
            FeedbackSignal = _inputs[feedbackIndex];
            var controlIndex = _settingsService.ControlIndex;
            controlIndex = controlIndex >= 0 && controlIndex < _outputs.Length ? controlIndex : 0;
            ControlSignal = _outputs[controlIndex];

            RegulatorKp = _settingsService.RegulatorKp;
            RegulatorKi = _settingsService.RegulatorKi;
            RegulatorKd = _settingsService.RegulatorKd;

            var safety = _settingsService.Safety;
            var len = Math.Min(safety.Length, _safety.Length);
            for (int i = 0; i < len; i++)
            {
                _safety[i].IsSafe = safety[i] == '1';
            }
        }

        private void SetStates()
        {
            CanStart = !_stopping && !_running;
            CanStop = _running;
        }
    }
}
