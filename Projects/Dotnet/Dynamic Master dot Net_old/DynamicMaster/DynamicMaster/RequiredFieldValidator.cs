using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace DynamicMaster
{
    class RequiredFieldValidator : Component
    {
        Control _controlToValidate;// { get; set; }
        string _errorMessage;// { get; set; }
        string _initialValue;// { get; set; }
        bool _isValid;// { get; set; }
        //void Validate();
        Icon _icon;
        ErrorProvider _errorProvider;

        Control ControlToValidate { get; set; }
        Icon Icon { get; set; }

        string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }
        Icon icon
        {
            get { return _icon; }
            set { _icon = value; }
        }
        string InitialValue
        {
            get { return _initialValue; }
            set { _initialValue = value; }
        }
        bool IsValid
        {
            get { return _isValid; }
            set { _isValid = value; }
        }
        void Validate()
        {
            // Is valid if different than initial value,
            // which is not necessarily an empty string
            string controlValue = ControlToValidate.Text.Trim();
            string initialValue;
            if (_initialValue == null) initialValue = "";
            else initialValue = _initialValue.Trim();
            _isValid = (controlValue != initialValue);
            // Display an error if ControlToValidate is invalid
            string errorMessage = "";
            if (!_isValid)
            {
                errorMessage = _errorMessage;
                _errorProvider.Icon = _icon;
            }
            _errorProvider.SetError(_controlToValidate, errorMessage);
        }
        private void ControlToValidate_Validating(
          object sender,
          CancelEventArgs e)
        {
            // We don't cancel if invalid since we don't want to force
            // the focus to remain on ControlToValidate if invalid
            Validate();
        }
    }
}
