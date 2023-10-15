using System;
using System.Collections;
using System.Collections.Generic;

namespace RapaxFructus
{
    /// <summary>
    /// ������������ ���� �������������� ������.
    /// </summary>
    public enum Language
    {
        Russian,
        English
    }
    /// <summary>
    /// �����-��������� ��� ����� ������ � ����, ��� �������� � ������.
    /// </summary>
    public static class Localization
    {
        private static Language _currentLanguage;
        
        private static Dictionary<string, Dictionary<Language, string>> _localization = new Dictionary<string, Dictionary<Language, string>>
        {
            {
                "",
                new Dictionary<Language, string>
                {
                    { Language.Russian, ""},
                    { Language.English, ""}
                }
            },
            {
                "Enemy<LittleBug>Name",
                new Dictionary<Language, string>
                {
                    { Language.Russian, "��������� ���"},
                    { Language.English, "Little Bug"}
                }
            },
            {
                "Enemy<LittleBug>Description",
                new Dictionary<Language, string>
                {
                    { Language.Russian, ""},
                    { Language.English, ""}
                }
            },
        };

        public delegate void LanguageHandler();
        /// <summary>
        /// ����������� ��� ����� �����.
        /// </summary>
        public static event LanguageHandler LanguageChanged; 

        /// <summary>
        /// �������� ������� �� ������� ����.
        /// </summary>
        /// <param name="key">
        /// �����-����.
        /// </param>
        /// <returns></returns>
        public static string Get(string key)
        {
            return _localization[key][_currentLanguage];
        }
        public static void ChangeLanguage(Language language)
        {
            if (language == _currentLanguage)
                return;
            _currentLanguage = language;
            LanguageChanged?.Invoke();
        }
    }
}