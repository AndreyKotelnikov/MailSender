using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace WpfMailSender.ViewModels
{
    public class EditDateTimeViewModel : ViewModelBase
    {
        private DateTime _dateTime;
        private DateTime _date;
        private DateTime _time;

        public EditDateTimeViewModel(DateTime dateTime)
        {
            _dateTime = dateTime;
            _date = _dateTime.Date;
            _time = _dateTime;
        }

        /// <summary>
        /// Only for design Mode
        /// </summary>
        public EditDateTimeViewModel() : this(DateTime.Now)
        {
        }

        public DateTime DateTime => _dateTime;

        public DateTime Date
        {
            get => _date;
            set
            {
                Set(ref _date, value);
                UpdateDateTime();
            }
        }

        public DateTime Time
        {
            get => _time;
            set
            {
                Set(ref _time, value);
                UpdateDateTime();
            }
        }

        private void UpdateDateTime()
        {
            _dateTime = new DateTime(
                _date.Year, 
                _date.Month, 
                _date.Day, 
                _time.Hour, 
                _time.Minute, 
                _time.Second
                );
        }
    }
}
