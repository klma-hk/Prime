using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WPFMT
{
    public class INPCBase : INotifyPropertyChanged , INotifyDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors
        {
            get { return _Errors.Count > 0; }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (_Errors.ContainsKey(propertyName))
            {
                return _Errors[propertyName];
            }
            return null;
        }

        protected Dictionary<string, List<string>> _Errors = new Dictionary<string, List<string>>();

        protected void ValidateProperty<T>(T value ,  [CallerMemberName] string propertyname = "")
        {
            var results = new List<ValidationResult>();
            ValidationContext context = new ValidationContext(this);
            context.MemberName = propertyname;
            Validator.TryValidateProperty(value, context, results);
            if (results.Any())
            {
                _Errors[propertyname] = results.Select(c => c.ErrorMessage).ToList();
            }
            else
            {
                _Errors.Remove(propertyname);
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyname));
        }

        protected void OnErrorsChanged(string propertyname)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyname));
        }

    }
}
