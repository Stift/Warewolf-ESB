﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.18444
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Warewolf.AcceptanceTesting.Explorer.WebService
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class WebSourceFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Web Source.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Web Source", "In order to create a web source for web services\r\nAs a Warewolf user\r\nI want to b" +
                    "e able to manage web sources easily", ProgrammingLanguage.CSharp, new string[] {
                        "WebSource"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((TechTalk.SpecFlow.FeatureContext.Current != null) 
                        && (TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title != "Web Source")))
            {
                Warewolf.AcceptanceTesting.Explorer.WebService.WebSourceFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Creating New Web Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Web Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("WebSource")]
        public virtual void CreatingNewWebSource()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Creating New Web Source", ((string[])(null)));
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
   testRunner.Given("I open New Web Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
   testRunner.And("I type Address as \"http://RSAKLFSVRTFSBLD/IntegrationTestSite\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 10
   testRunner.And("I type Default Query as \"/GetCountries.ashx?extension=json&prefix=a\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
   testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 13
   testRunner.Then("\"View Results in Browser\" is \"Invisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 14
   testRunner.And("\"TestDefault\" is \"Invisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 15
   testRunner.And("I Select Authentication Type as \"Anonymous\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 16
   testRunner.And("Username field is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 17
   testRunner.And("Password field is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 18
   testRunner.When("Test Connecton is \"Successful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 19
   testRunner.Then("\"View Results in Browser\" is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 20
   testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
   testRunner.And("\"TestDefault\" is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
   testRunner.When("I save the source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 23
   testRunner.Then("the save dialog is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Creating New Web Source under auth type as user")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Web Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("WebSource")]
        public virtual void CreatingNewWebSourceUnderAuthTypeAsUser()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Creating New Web Source under auth type as user", ((string[])(null)));
#line 26
this.ScenarioSetup(scenarioInfo);
#line 27
   testRunner.Given("I open New Web Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 28
   testRunner.And("I type Address as \"http://RSAKLFSVRTFSBLD/IntegrationTestSite\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 29
   testRunner.And("I type Default Query as \"/GetCountries.ashx?extension=json&prefix=a\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 30
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 31
   testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 32
   testRunner.Then("\"View Results in Browser\" is \"Invisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 33
   testRunner.And("\"TestDefault\" is \"Invisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 34
   testRunner.And("I Select Authentication Type as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 35
   testRunner.And("Username field is \"IntegrationTester\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 36
   testRunner.And("Password field is \"I73573r0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 37
   testRunner.When("Test Connecton is \"Successful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 38
   testRunner.Then("\"View Results in Browser\" is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 39
   testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 40
   testRunner.And("\"TestDefault\" is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 41
   testRunner.When("I save the source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 42
   testRunner.Then("the save dialog is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Incorrect address windows auth type not allowing save")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Web Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("WebSource")]
        public virtual void IncorrectAddressWindowsAuthTypeNotAllowingSave()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Incorrect address windows auth type not allowing save", ((string[])(null)));
#line 45
this.ScenarioSetup(scenarioInfo);
#line 46
   testRunner.Given("I open New Web Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 47
   testRunner.And("I type Address as \"sdfsdfd\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 48
   testRunner.And("I type Default Query as \"/GetCountries.ashx?extension=json&prefix=a\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 49
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
   testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 51
   testRunner.Then("\"View Results in Browser\" is \"Invisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 52
   testRunner.And("\"TestDefault\" is \"Invisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 53
   testRunner.And("I Select Authentication Type as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 54
   testRunner.And("Username field is \"IntegrationTester\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 55
   testRunner.And("Password field is \"I73573r0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 56
   testRunner.When("Test Connecton is \"UnSuccessful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 57
   testRunner.And("Validation message is thrown", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 58
   testRunner.Then("\"View Results in Browser\" is \"Not Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 59
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 60
   testRunner.And("\"TestDefault\" is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Incorrect address user auth type is not allowing to save")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Web Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("WebSource")]
        public virtual void IncorrectAddressUserAuthTypeIsNotAllowingToSave()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Incorrect address user auth type is not allowing to save", ((string[])(null)));
#line 62
this.ScenarioSetup(scenarioInfo);
#line 63
   testRunner.Given("I open New Web Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 64
   testRunner.And("I type Address as \"sdfsdfd\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 65
   testRunner.And("I type Default Query as \"/GetCountries.ashx?extension=json&prefix=a\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 66
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 67
   testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 68
   testRunner.Then("\"View Results in Browser\" is \"Invisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 69
   testRunner.And("\"TestDefault\" is \"Invisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 70
   testRunner.And("I Select Authentication Type as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 71
   testRunner.And("Username field is \"test\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 72
   testRunner.And("Password field is \"I73573r0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 73
   testRunner.When("Test Connecton is \"UnSuccessful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 74
   testRunner.And("Validation message is thrown", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 75
   testRunner.Then("\"View Results in Browser\" is \"Not Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 76
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 77
   testRunner.And("\"TestDefault\" is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Testing Auth type as Windows and swaping it resets the test connection")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Web Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("WebSource")]
        public virtual void TestingAuthTypeAsWindowsAndSwapingItResetsTheTestConnection()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Testing Auth type as Windows and swaping it resets the test connection", ((string[])(null)));
#line 80
this.ScenarioSetup(scenarioInfo);
#line 81
   testRunner.Given("I open New Web Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 82
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 83
   testRunner.And("I type Address as \"http://RSAKLFSVRTFSBLD/IntegrationTestSite\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 84
   testRunner.And("I type Default Query as \"/GetCountries.ashx?extension=json&prefix=a\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 85
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 86
   testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 87
   testRunner.Then("\"View Results in Browser\" is \"Invisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 88
   testRunner.And("\"TestDefault\" is \"Invisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 89
   testRunner.And("I Select Authentication Type as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 90
   testRunner.And("Username field is \"test\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 91
   testRunner.And("Password field is \"I73573r0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 92
   testRunner.When("Test Connecton is \"Successful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 93
   testRunner.And("Validation message is Not thrown", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 94
   testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 95
   testRunner.And("I Select Authentication Type as \"Anonymous\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 96
   testRunner.And("Username field is \"Invisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 97
   testRunner.And("Password field is \"Invisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 98
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 99
   testRunner.When("Test Connecton is \"Successful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 100
   testRunner.And("Validation message is Not thrown", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 101
   testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 102
   testRunner.And("I Select Authentication Type as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 103
   testRunner.And("Username field is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 104
   testRunner.And("Password field is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 105
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Editing saved Web Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Web Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("WebSource")]
        public virtual void EditingSavedWebSource()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Editing saved Web Source", ((string[])(null)));
#line 108
this.ScenarioSetup(scenarioInfo);
#line 109
   testRunner.Given("I open \"Edit Source - Test\" web source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 110
   testRunner.And("Address is \"http://RSAKLFSVRTFSBLD/IntegrationTestSite\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 111
   testRunner.And("Default Query is \"/GetCountries.ashx?extension=json&prefix=a\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 112
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 113
   testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 114
   testRunner.Then("\"View Results in Browser\" is \"Invisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 115
   testRunner.And("\"TestDefault\" is \"Invisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 116
   testRunner.And("Select Authentication Type as \"Anonymous\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 117
   testRunner.And("Username field is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 118
   testRunner.And("Password field is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 119
   testRunner.When("Test Connecton is \"Successful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 120
   testRunner.Then("\"View Results in Browser\" is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 121
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 122
   testRunner.And("\"TestDefault\" is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 123
   testRunner.When("I change Address to \"http://RSAKLFSVRTFSBLD/IntegrationTestSite\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 124
   testRunner.And("I change Default Query to \"/GetCountries.ashx?extensio", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 125
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 126
   testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 127
   testRunner.Then("\"View Results in Browser\" is \"Invisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 128
   testRunner.And("\"TestDefault\" is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 129
   testRunner.Then("\"View Results in Browser\" is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 130
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 131
   testRunner.When("Test Connecton is \"Successfull\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 132
   testRunner.Then("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Editing saved Web Source auth type")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Web Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("WebSource")]
        public virtual void EditingSavedWebSourceAuthType()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Editing saved Web Source auth type", ((string[])(null)));
#line 136
this.ScenarioSetup(scenarioInfo);
#line 137
   testRunner.Given("I open \"Edit Source - Test\" web source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 138
   testRunner.And("Address as \"http://RSAKLFSVRTFSBLD/IntegrationTestSite\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 139
   testRunner.And("Default Query as \"/GetCountries.ashx?extension=json&prefix=a\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 140
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 141
   testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 142
   testRunner.Then("\"View Results in Browser\" is \"Invisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 143
  testRunner.And("\"TestDefault\" is \"Invisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 144
   testRunner.And("Select Authentication Type as \"Anonymous\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 145
   testRunner.And("Username field is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 146
   testRunner.And("Password field is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 147
   testRunner.When("Test Connecton is \"Successful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 148
   testRunner.Then("\"View Results in Browser\" is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 149
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 150
   testRunner.And("\"TestDefault\" is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 151
   testRunner.When("I edit Authentication Type as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 152
   testRunner.And("Username field is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 153
   testRunner.And("Password field is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 154
   testRunner.When("Test Connecton is \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 155
   testRunner.And("Username field is \"IntegrationTester\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 156
   testRunner.And("Password field is \"I73573r0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 157
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 158
   testRunner.When("Test Connecton is \"Successfull\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 159
   testRunner.Then("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
