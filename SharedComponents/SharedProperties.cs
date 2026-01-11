using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mitchell.ScmConsoles.SharedProperties
{
   
   
    public class TokenNameValue
    {
        public string TOKENNAME { get; set; }
        public string TOKENVALUE { get; set; }
     
    }
  
    public class SettingsEnvToken
    {
        private string settingsEnvFile;
        public string Name { get; set; }
        public string Value { get; set; }
        public string Value_UnwrappedContent { get; set; }
        public string Encrypted { get; set; }
        public string FoundInSettingsEnvFile 
        { 
            get
            {
                return settingsEnvFile;
            }
            set
            {
                settingsEnvFile = value;

                //there are situation that the path contains forward dash and backward dash
                //C:\automation\tokens\US/APD/DART_GMR/DEV\SettingsEnv.txt
                //therefore, we want to convert them all into backward dash
                settingsEnvFile = settingsEnvFile.Replace('/', '\\');

                //the path to any found file is: C:\automation\Tokens\US\APD\IIS_GMR_JBOSS_2012\DEV\SettingsEnv.xml
                string[] directoryNames = settingsEnvFile.Split('\\');
                if (directoryNames.Count() > 5)
                {
                    //therefore, after the split, get the EnvironmentTargetName as "DEV"
                    TargetEnvironment = directoryNames[directoryNames.Count() - 2];

                    //get the environmentName as "IIS_GMR_JBOSS_2012"
                    EnvironmentPool = directoryNames[directoryNames.Count() - 3];

                    //get the Business Unit as "APD"
                    BusinessUnit = directoryNames[directoryNames.Count() - 4];

                    //get the Region as "US"
                    Region = directoryNames[directoryNames.Count() - 5];
                }

            }
        }
        public string TargetEnvironment { get; set;} //Target environment such as DEV, CI or QA
        public string EnvironmentPool { get; set; } //Environment pool such as IIS_GMR_JBOSS_2012
        public string BusinessUnit { get; set; } //Business Unit environment such as APD, ACS
        public string Region { get; set; } //Region environment such as US, EU
  
    }


}