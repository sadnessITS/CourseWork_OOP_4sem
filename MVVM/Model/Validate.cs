using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace HospitalPatientRecords.MVVM.Model
{
    class IdAttribute : ValidationAttribute
    {
        Regex regex = new Regex(@"(?=(^([^\d]*?\d){6}$))");
        public override bool IsValid(object? value)
        {
            if (regex.IsMatch(value.ToString())) return true;
            else ErrorMessage = "Personal Number is wrong!!!";
            
            return false;
        }
    }
    class FioAttribute : ValidationAttribute
    {
        Regex regex = new Regex(@"[a-zA-z]");
        public override bool IsValid(object? value)
        {
            if (regex.IsMatch(value.ToString())) return true;
            else ErrorMessage = "Name is wrong!!!";
            
            return false;
        }
    }
    class Attribute : ValidationAttribute
    {
        Regex regex = new Regex(@"[a-zA-Zа-яА-Я]");
        public override bool IsValid(object? value)
        {
            if (regex.IsMatch(value.ToString())) return true;
            else ErrorMessage = "Name is wrong!!!";
            
            return false;
        }
    }
    class AgeAttribute : ValidationAttribute
    {
        Regex regex = new Regex(@"^(100|[1-9]?\d)$");
        public override bool IsValid(object? value)
        {
            if (regex.IsMatch(value.ToString())) return true;
            else ErrorMessage = "Age is wrong!!!";
            
            return false;
        }
    }
    class ResidencyAttribute : ValidationAttribute
    {
        Regex regex = new Regex(@"[a-zA-Zа-яА-Я]");
        public override bool IsValid(object? value)
        {
            if (regex.IsMatch(value.ToString())) return true;
            else ErrorMessage = "Residency is wrong!!!";
            
            return false;
        }
    }
    class CopyPapersAttribute : ValidationAttribute
    {
        Regex regex = new Regex(@"[a-zA-Zа-яА-Я]");
        public override bool IsValid(object? value)
        {
            if (regex.IsMatch(value.ToString())) return true;
            else ErrorMessage = "Papers destination is wrong!!!";
            
            return false;
        }
    }
    class TextAttribute : ValidationAttribute
    {
        Regex regex = new Regex(@"^(.{1,33}|[^0-9]*[A-Z]*[А-Я])$");
        public override bool IsValid(object? value)
        {
            if (regex.IsMatch(value.ToString())) return true;
            else ErrorMessage = "Password is wrong!!!";
            
            return false;
        }
    }
}