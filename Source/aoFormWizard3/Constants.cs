
namespace Contensive.Addon.aoFormWizard3 {

    public static class Constants {
        // 
        // -- layout
        public const string guidLayoutFormWizard = "{8DCD239D-9E44-4EA9-AC17-C5BB934FAA9A}";
        public const string nameLayoutFormWizard = "Form Wizard Layout";
        public const string pathFilenameLayoutFormWizard = "formwizard/FormWizardLayout.html";
        //
        public const string guidAddonRecaptchav2 = "{500A1F57-86A2-4D47-B747-4EF4D30A83E2}";
        // 
        // -- version used to upgrade content
        public const int version = 2;
        // Field types. This enum is taken from contensive 5 code
        public enum FieldTypeIdEnum {
            IntegerType = 1,
            Text = 2,
            LongText = 3,
            BooleanType = 4,
            DateType = 5,
            File = 6,
            Lookup = 7,
            Redirect = 8,
            Currency = 9,
            FileText = 10,
            FileImage = 11,
            Float = 12,
            AutoIdIncrement = 13,
            ManyToMany = 14,
            MemberSelect = 15,
            FileCSS = 16,
            FileXML = 17,
            FileJavascript = 18,
            Link = 19,
            ResourceLink = 20,
            HTML = 21,
            FileHTML = 22,
            HTMLCode = 23,
            FileHTMLCode = 24
        }
    }
}