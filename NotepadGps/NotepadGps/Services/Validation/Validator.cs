﻿using System.Text.RegularExpressions;

namespace NotepadGps.Services.Validation
{
    public class Validator
    {
        private const string NameReg = @"^(?=.*[A-Za-z0-9]$)[A-Za-z][A-Za-z\d.-]{3,16}$";
        private const string EmailReg = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]{1,9}\.)+[a-zA-Z]{2,3})|(([0-9]{2,3}\.){2}[0-9]{2,3}))\z";
        private const string PasswordReg = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)([a-zA-Z\d]){8,16}$";

        static Validator()
        {
            Name = new Validator(NameReg);
            Email = new Validator(EmailReg);
            Password = new Validator(PasswordReg);
        }

        private Validator(string pattern)
        {
            Pattern = pattern;
        }

        public static bool StringValid(string input, Validator valid)
        {
            return !string.IsNullOrEmpty(input) &&
                Regex.IsMatch(input, valid.Pattern);
        }

        public static Validator Name { get; }
        public static Validator Email { get; }
        public static Validator Password { get; }

        private string Pattern { get; }
    }
}

