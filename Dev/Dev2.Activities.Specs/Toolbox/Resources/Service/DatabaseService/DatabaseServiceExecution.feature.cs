﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.18063
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Dev2.Activities.Specs.Toolbox.Resources.Service.DatabaseService
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class DatabaseServiceExecutionFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "DatabaseServiceExecution.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "DatabaseServiceExecution", "In order to use Database service \r\nAs a Warewolf user\r\nI want a tool that calls t" +
                    "he Database services into the workflow", ProgrammingLanguage.CSharp, ((string[])(null)));
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
                        && (TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title != "DatabaseServiceExecution")))
            {
                Dev2.Activities.Specs.Toolbox.Resources.Service.DatabaseService.DatabaseServiceExecutionFeature.FeatureSetup(null);
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
        
        public virtual void ExecutingDatabaseServiceUsingNumericIndexesAndScalar(string nameVariable, string emailVariable, string errorOccured, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "mytag"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Executing Database service using numeric indexes and scalar", @__tags);
#line 8
this.ScenarioSetup(scenarioInfo);
#line 9
      testRunner.Given("I have a database service \"mails\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Email"});
            table1.AddRow(new string[] {
                        string.Format("{0}", nameVariable),
                        string.Format("{0}", emailVariable)});
#line 10
      testRunner.And("the output is mapped as", ((string)(null)), table1, "And ");
#line 13
      testRunner.When("the Service is executed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 14
      testRunner.Then(string.Format("the execution has \"{0}\" error", errorOccured), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        ""});
            table2.AddRow(new string[] {
                        string.Format("{0}  = String", nameVariable)});
            table2.AddRow(new string[] {
                        string.Format("{0} = String", emailVariable)});
#line 15
      testRunner.And("the debug output as", ((string)(null)), table2, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Executing Database service using numeric indexes and scalar")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DatabaseServiceExecution")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("mytag")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "[[rec(1).name]]")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:nameVariable", "[[rec(1).name]]")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:emailVariable", "[[rec(1).email]]")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:errorOccured", "NO")]
        public virtual void ExecutingDatabaseServiceUsingNumericIndexesAndScalar_Rec1_Name()
        {
            this.ExecutingDatabaseServiceUsingNumericIndexesAndScalar("[[rec(1).name]]", "[[rec(1).email]]", "NO", ((string[])(null)));
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Executing Database service using numeric indexes and scalar")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DatabaseServiceExecution")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("mytag")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "[[rec(2).name]]")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:nameVariable", "[[rec(2).name]]")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:emailVariable", "[[rec(2).email]]")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:errorOccured", "NO")]
        public virtual void ExecutingDatabaseServiceUsingNumericIndexesAndScalar_Rec2_Name()
        {
            this.ExecutingDatabaseServiceUsingNumericIndexesAndScalar("[[rec(2).name]]", "[[rec(2).email]]", "NO", ((string[])(null)));
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Executing Database service using numeric indexes and scalar")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DatabaseServiceExecution")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("mytag")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "[[rec(3).name]]")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:nameVariable", "[[rec(3).name]]")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:emailVariable", "[[rec(3).email]]")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:errorOccured", "NO")]
        public virtual void ExecutingDatabaseServiceUsingNumericIndexesAndScalar_Rec3_Name()
        {
            this.ExecutingDatabaseServiceUsingNumericIndexesAndScalar("[[rec(3).name]]", "[[rec(3).email]]", "NO", ((string[])(null)));
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Executing Database service using numeric indexes and scalar")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DatabaseServiceExecution")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("mytag")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "[[name]]")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:nameVariable", "[[name]]")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:emailVariable", "[[email]]]]")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:errorOccured", "NO")]
        public virtual void ExecutingDatabaseServiceUsingNumericIndexesAndScalar_Name()
        {
            this.ExecutingDatabaseServiceUsingNumericIndexesAndScalar("[[name]]", "[[email]]]]", "NO", ((string[])(null)));
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Executing Database service with recordset using star.")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DatabaseServiceExecution")]
        public virtual void ExecutingDatabaseServiceWithRecordsetUsingStar_()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Executing Database service with recordset using star.", ((string[])(null)));
#line 27
this.ScenarioSetup(scenarioInfo);
#line 28
     testRunner.Given("I have a DataBase service \"mails\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 29
     testRunner.And("the output is mapped as \"[[rec(*).name]]\" and \"[[rec(*).email]]\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 30
     testRunner.When("the Service is executed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 31
     testRunner.Then("the execution has \"NO\" error", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        ""});
            table3.AddRow(new string[] {
                        "[[rec(1).name]]  = Tshepo"});
            table3.AddRow(new string[] {
                        "[[rec(2).name]]  = Hags"});
            table3.AddRow(new string[] {
                        "[[rec(3).name]]  = Ashley"});
            table3.AddRow(new string[] {
                        "[[rec(1).email]] = tshepo.ntlhokoa@dev2.co.za"});
            table3.AddRow(new string[] {
                        "[[rec(2).email]] = hagashen.naidu@dev2.co.za"});
            table3.AddRow(new string[] {
                        "[[rec(3).email]] = ashley.lewis@dev2.co.za"});
#line 32
     testRunner.And("the debug output as", ((string)(null)), table3, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Executing Email service with negative recordset in name.")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DatabaseServiceExecution")]
        public virtual void ExecutingEmailServiceWithNegativeRecordsetInName_()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Executing Email service with negative recordset in name.", ((string[])(null)));
#line 42
this.ScenarioSetup(scenarioInfo);
#line 43
       testRunner.Given("I have a DataBase service \"mails\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 44
       testRunner.And("the output is mapped as \"[[rec(-1).name]]\" and \"[[rec(*).email]]\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 45
       testRunner.When("the Service is executed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 46
       testRunner.Then("the execution has \"AN\" error", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        ""});
            table4.AddRow(new string[] {
                        "[[rec(-1).name]]  ="});
            table4.AddRow(new string[] {
                        "[[rec(1).email]] = tshepo.ntlhokoa@dev2.co.za"});
            table4.AddRow(new string[] {
                        "[[rec(1).email]] = hagashen.naidu@dev2.co.za"});
            table4.AddRow(new string[] {
                        "[[rec(3).email]] = ashley.lewis@dev2.co.za"});
#line 47
       testRunner.And("the debug output as", ((string)(null)), table4, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Executing Email service with negative recordset in email.")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DatabaseServiceExecution")]
        public virtual void ExecutingEmailServiceWithNegativeRecordsetInEmail_()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Executing Email service with negative recordset in email.", ((string[])(null)));
#line 54
this.ScenarioSetup(scenarioInfo);
#line 55
      testRunner.Given("I have DataBase service \"mails\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 56
      testRunner.And("the output as  \"[[rec(1).name]]\" and  \"[[rec(-1).email]]\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 57
      testRunner.When("the Service is executed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 58
      testRunner.Then("the execution has \"AN\" error", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "[[rec(1).name]]   =  Tsepho"});
            table5.AddRow(new string[] {
                        "Email",
                        "[[rec(-1).email]] ="});
#line 59
      testRunner.And("the debug output as", ((string)(null)), table5, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Executing \'country service\' with valid input.")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DatabaseServiceExecution")]
        public virtual void ExecutingCountryServiceWithValidInput_()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Executing \'country service\' with valid input.", ((string[])(null)));
#line 64
this.ScenarioSetup(scenarioInfo);
#line 65
      testRunner.Given("I have a DataBase service \"country\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 66
      testRunner.And("the Input as \"Alb\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 67
   testRunner.And("the output varaibles as \"[[country]]\" and \"[[Id]]\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 68
      testRunner.When("the Service is executed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 69
      testRunner.Then("the execution has \"NO\" error", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Alb"});
#line 70
      testRunner.And("the debug input as", ((string)(null)), table6, "And ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "",
                        ""});
            table7.AddRow(new string[] {
                        "[[country]]",
                        "Albania"});
            table7.AddRow(new string[] {
                        "[[Id]]",
                        "2"});
#line 72
       testRunner.And("the debug output as", ((string)(null)), table7, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Executing \'country service\' with Invalid input.")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DatabaseServiceExecution")]
        public virtual void ExecutingCountryServiceWithInvalidInput_()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Executing \'country service\' with Invalid input.", ((string[])(null)));
#line 77
this.ScenarioSetup(scenarioInfo);
#line 78
        testRunner.Given("I have a DataBase service \"country\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 79
        testRunner.And("the Input as \"jajjj\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 80
  testRunner.And("the output varaibles as \"[[country]]\" and \"[[Id]]\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 81
        testRunner.When("the Service is executed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 82
        testRunner.Then("the execution has \"AN\" error", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "jajjj"});
#line 83
        testRunner.And("the debug input as", ((string)(null)), table8, "And ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        ""});
            table9.AddRow(new string[] {
                        "[[country]]"});
            table9.AddRow(new string[] {
                        "[[Id]]"});
#line 85
        testRunner.And("the debug output as", ((string)(null)), table9, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Executing \'insertdummyuser\' with input.")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DatabaseServiceExecution")]
        public virtual void ExecutingInsertdummyuserWithInput_()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Executing \'insertdummyuser\' with input.", ((string[])(null)));
#line 90
this.ScenarioSetup(scenarioInfo);
#line 91
       testRunner.Given("I have a DataBase service \"insertdummyuser\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 92
       testRunner.And("input name as \"Murali\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 93
       testRunner.And("input  Iname as \"Naidu\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 94
       testRunner.And("input username as \"Murali1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 95
       testRunner.And("input password as \"I can\'t say\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 96
       testRunner.And("input lastAccessDate as \"22/01/1992\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 97
       testRunner.Then("the execution has \"NO\" error", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "fname",
                        "Iname",
                        "username",
                        "password",
                        "lastAccessDate"});
            table10.AddRow(new string[] {
                        "Murali",
                        "Naidu",
                        "Murali1",
                        "I can\'t say",
                        "22/01/1992"});
#line 98
       testRunner.And("the debug input as", ((string)(null)), table10, "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
