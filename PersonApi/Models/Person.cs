using Newtonsoft.Json;
using System.Net;
using System.Text.Json.Serialization;

namespace PersonApi.Models
{
    //[JsonObject(MemberSerialization.OptIn)]
    public class Person
    {
        public long Id { get; set; }
        [JsonProperty]
        public string FirstName { get; set; }
        [JsonProperty]
        public string LastName { get; set; }
        [JsonProperty]
        public String[] Skills { get; set; }
        [JsonProperty]
        public List<SocialAccount> Accounts { get; set; } = new List<SocialAccount>();


        private int NumberVowels { get; set; }
        private int NumberConsonants { get; set; }
        private string ReversedName {  get; set; }
        //private string Json { get; set; }


        // Function to count vowels and consonants in person name
        public void countVowelsAndConsonants()
        {
            int i;
            int vowels = 0;
            int consonants = 0;
            string name = FirstName + LastName;
            for (i = 0; i< name.Length; i++)
            {
                if (name[i] == 'a' || name[i] == 'e' || name[i] == 'i' || name[i] == 'o' || name[i] == 'u' || name[i] == 'A' || name[i] == 'E' || name[i] == 'I' || name[i] == 'O' || name[i] == 'U')
                {
                    vowels++;
                }
                else if ((name[i] >= 'a' && name[i] <= 'z') || (name[i] >= 'A' && name[i] <= 'Z'))
                {
                    consonants++;
                }
            }
            this.NumberVowels = vowels;
            this.NumberConsonants = consonants;
        }

        // Function setReversedName to create the reversed name for person 
        public void setReversedName ()
        {
            String name = FirstName + " " + LastName; 
            char[] charArray = name.ToCharArray();
            string reversedString = String.Empty;
            for (int i = charArray.Length - 1; i > -1; i--)
            {

                reversedString += charArray[i];
            }
            ReversedName = reversedString;
        }

        /* Function ConsoleOutput to print information to console, see example output:
         
        The number of VOWELS: 3
        The number of CONSTENANTS: 4
        The firstname + last name entered: John Doe
        The reverse version of the firstname and lastname: eoD nhoJ
        The JSON format of the entire object:
        {   
            "FirstName": "John",
            "LastName": "Doe",
            "SocialSkills": [
                "social",
                "fun",
                "coach"
            ],
            "SocialAccounts": [
                {
                    "Type": "Twitter",
                    "Address": "@JohnDoe"
                },
                {
                    "Type": "Linkedin",
                    "Address": "Linkedin.com/johndoe"
                }
            ]
        }
         */
        public void ConsoleOutput ()
        {
            Console.WriteLine ($"The Number of VOWELS: {NumberVowels}");
            Console.WriteLine($"The Number of CONSONANTS: {NumberConsonants}");
            Console.WriteLine($"The firstname + last name entered: {FirstName} {LastName}");
            Console.WriteLine($"The reverse version of the fristname and lastname: {ReversedName}");
            Console.WriteLine("The JSON format of the entire object:");
            Console.WriteLine(JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}
