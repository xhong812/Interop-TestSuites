namespace Microsoft.Protocols.TestSuites.MS_SITESS
{
    using System;
    using System.Web.Services.Protocols;
    using Microsoft.Protocols.TestSuites.Common;
    using Microsoft.Protocols.TestTools;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The partial test class contains test case definitions related to ExportWorkflowTemplate operation.
    /// </summary>
    [TestClass]
    public class S05_ExportWorkflowTemplate : TestClassBase
    {
        /// <summary>
        /// An instance of protocol adapter class.
        /// </summary>
        private IMS_SITESSAdapter sitessAdapter;

        /// <summary>
        /// An instance of SUT control adapter class.
        /// </summary>
        private IMS_SITESSSUTControlAdapter sutAdapter;

        /// <summary>
        /// The name of a solution package which is to be exported, which is a compressed file that can be deployed to a server farm or a site.
        /// </summary>
        private string solutionName = string.Empty;

        /// <summary>
        /// A boolean value indicates whether the solution file is generated by the ExportWorkflowTemplate operation.
        /// </summary>
        private bool isSolutionFileCreated = false;

        #region Test Suite Initialization & Cleanup

        /// <summary>
        /// Test Suite Initialization.
        /// </summary>
        /// <param name="testContext">The test context.</param>
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
        }

        /// <summary>
        /// Test Suite Cleanup.
        /// </summary>
        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }

        #endregion

        #region Test Cases

        #region Scenario 5 ExportWorkflowTemplate

        /// <summary>
        /// This test case is designed to verify the successful status of ExportWorkflowTemplate operation.
        /// </summary>
        [TestCategory("MSSITESS"), TestMethod()]
        public void MSSITESS_S05_TC01_ExportWorkflowTemplateSucceed()
        {
            Site.Assume.IsTrue(Common.IsRequirementEnabled(5331, this.Site), @"Test is executed only when R5331Enabled is set to true.");

            #region Variables
            string templateName = Common.GetConfigurationPropertyValue(Constants.WorkflowTemplateName, this.Site);
            string destinationUrl = Common.GetConfigurationPropertyValue(Constants.DataPath, this.Site);
            string exportResult = string.Empty;
            string[] exportFiles = null;
            string[] expectedFiles = null;

            #endregion Variables

            // Initialize the web service with an authenticated account.
            this.sitessAdapter.InitializeWebService(UserAuthenticationOption.Authenticated);

            // Invoke ExportWorkflowTemplate operation, a URL is expected to be returned and a solution file is expected to be exported.
            exportResult = this.sitessAdapter.ExportWorkflowTemplate(this.solutionName + Constants.WspExtension, Constants.SolutionTitle, Constants.SolutionDescription, templateName, destinationUrl);

            // If returned value is not a URL or exported files are inconsistent with the expected, log it.
            Site.Assert.IsTrue(
                Uri.IsWellFormedUriString(exportResult, UriKind.Relative),
                "ExportWorkflowTemplate should return a valid Uri, actual uri {0}.",
                exportResult);

            string solutions = TestSuiteHelper.VerifyExportAndImportFile(this.solutionName, 1, this.Site, this.sutAdapter);
            exportFiles = solutions == null ? null : solutions.TrimEnd(new char[] { ';' }).Split(';');
            this.isSolutionFileCreated = true;

            // Format the expected file names in the document library, only one solution file (i.e. SolutionName) is expected.
            expectedFiles = new string[] { this.solutionName + Constants.WspExtension };

            Site.Assert.IsTrue(
                AdapterHelper.CompareStringArrays(expectedFiles, exportFiles),
                "ExportWorkflowTemplate should export the solution file.");

            // If returned value is a URL and exported files are consistent with the expected, it means the ExportWorkflowTemplate operation succeed.
            // Invoke ExportWorkflowTemplate operation again, a URL is expected to be returned and a second solution file is expected to be exported.
            exportResult = this.sitessAdapter.ExportWorkflowTemplate(this.solutionName + Constants.WspExtension, Constants.SolutionTitle, Constants.SolutionDescription, templateName, destinationUrl);

            // If returned value is not a URL or exported files are inconsistent with the expected, log it.
            Site.Assert.IsTrue(
                Uri.IsWellFormedUriString(exportResult, UriKind.Relative),
                "ExportWorkflowTemplate should return a valid Uri, actual uri {0}.",
                exportResult);

            solutions = TestSuiteHelper.VerifyExportAndImportFile(this.solutionName, 2, this.Site, this.sutAdapter);
            exportFiles = solutions == null ? null : solutions.TrimEnd(new char[] { ';' }).Split(';');

            // Format the expected file names in the document library, two solution files (i.e. SolutionName & SolutionName2) are expected.
            expectedFiles = new string[]
                {
                        this.solutionName + Constants.WspExtension,
                        this.solutionName + "2" + Constants.WspExtension
                };

            #region Capture requirements

            bool isFilesAreEqual = AdapterHelper.CompareStringArrays(expectedFiles, exportFiles);

            // If the exported files are consistent with the expected, R386 can be captured.
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-SITESS_R386, the export files are {0}", exportFiles);

            // Verify MS-SITESS requirement: MS-SITESS_R386
            Site.CaptureRequirementIfIsTrue(
                isFilesAreEqual,
                386,
                @"[In ExportWorkflowTemplate] [solutionFileName:] If a solution with the specified name already exists in the document library in which the solution file needs to be created, the server MUST retry with <filename>2.wsp, where <filename> is obtained from solutionFileName after excluding the extension.");

            // If the exported files are consistent with the expected, R109 can be captured.
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-SITESS_R109, the export files are {0}", exportFiles);

            // Verify MS-SITESS requirement: MS-SITESS_R109
            Site.CaptureRequirementIfIsTrue(
                isFilesAreEqual,
                109,
                @"[In ExportWorkflowTemplate] [solutionFileName:] If a unique name is obtained, the protocol server MUST continue with that name [to create a solution file using this unique name].");

            // If code can run to here, it means that Microsoft SharePoint Foundation 2010 and above support operation ExportWorkflowTemplate.
            this.VerifyOperationExportWorkflowTemplate();
            #endregion Capture requirements
        }

        /// <summary>
        /// This test case is designed to verify ExportWorkflowTemplate when the library name is invalid.
        /// </summary>
        [TestCategory("MSSITESS"), TestMethod()]
        public void MSSITESS_S05_TC02_ExportWorkflowTemplateInvalidLibrary()
        {
            Site.Assume.IsTrue(Common.IsRequirementEnabled(5331, this.Site), @"Test is executed only when R5331Enabled is set to true.");

            #region Variables
            string templateName = Common.GetConfigurationPropertyValue(Constants.WorkflowTemplateName, this.Site);
            string libraryName = Common.GetConfigurationPropertyValue(Constants.InvalidLibraryName, this.Site);
            string destinationUrl = Common.GetConfigurationPropertyValue(Constants.SiteCollectionPath, this.Site) + "/" + libraryName;
            bool isErrorOccured = false;
            string errorCode = string.Empty;

            #endregion Variables

            // Initialize the web service with an authenticated account.
            this.sitessAdapter.InitializeWebService(UserAuthenticationOption.Authenticated);

            // Try to invoke ExportWorkflowTemplate operation with invalid template name, so an exception is expected.
            try
            {
                this.sitessAdapter.ExportWorkflowTemplate(
                    this.solutionName + Constants.WspExtension,
                    Constants.SolutionTitle,
                    Constants.SolutionDescription,
                    templateName,
                    destinationUrl);

                Site.Log.Add(LogEntryKind.Comment, "ExportWorkflowTemplate succeed!");
            }
            catch (SoapException e)
            {
                isErrorOccured = true;
                errorCode = Common.ExtractErrorCodeFromSoapFault(e);

                #region Capture requirements
                // Verify errorCode when soap exception is thrown.
                this.VerifySoapFault(isErrorOccured);
                this.VerifySoapFaultDetail(e);
                #endregion Capture requirements
            }

            #region Capture requirements
            // If an exception is thrown, R397 can be captured.
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-SITESS_R397, the error code is {0}", errorCode);

            // Verify MS-SITESS requirement: MS-SITESS_R397
            bool isVerifyR397 = isErrorOccured && string.IsNullOrEmpty(errorCode);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR397,
                397,
                @"[In ExportWorkflowTemplate] [destinationListUrl:] If a document library with the specified name does not exist, the server MUST return a SOAP fault with no error code.");

            // If code can run to here, it means that Microsoft SharePoint Foundation 2010 and above support operation ExportWorkflowTemplate.
            this.VerifyOperationExportWorkflowTemplate();
            #endregion Capture requirements
        }

        /// <summary>
        /// This test case is used to verify the ExportWorkflowTemplate when Template name is invalid.
        /// </summary>
        [TestCategory("MSSITESS"), TestMethod()]
        public void MSSITESS_S05_TC03_ExportWorkflowTemplateInvalidTemplate()
        {
            Site.Assume.IsTrue(Common.IsRequirementEnabled(5331, this.Site), @"Test is executed only when R5331Enabled is set to true.");

            #region Variables
            string templateName = Common.GetConfigurationPropertyValue(Constants.InvalidWorkflowTemplateName, this.Site);
            string destinationUrl = Common.GetConfigurationPropertyValue(Constants.DataPath, this.Site);
            bool isErrorOccured = false;

            #endregion Variables

            // Initialize the web service with an authenticated account.
            this.sitessAdapter.InitializeWebService(UserAuthenticationOption.Authenticated);

            // Try to invoke ExportWorkflowTemplate operation with invalid template name, so an exception is expected.
            try
            {
                this.sitessAdapter.ExportWorkflowTemplate(
                    this.solutionName + Constants.WspExtension,
                    Constants.SolutionTitle,
                    Constants.SolutionDescription,
                    templateName,
                    destinationUrl);

                Site.Log.Add(LogEntryKind.Comment, "ExportWorkflowTemplate succeed!");
            }
            catch (SoapException e)
            {
                isErrorOccured = true;

                #region Capture requirements
                this.VerifySoapFault(isErrorOccured);
                this.VerifySoapFaultDetail(e);
                #endregion Capture requirements
            }

            #region Capture requirements
            // If an exception is thrown, R396 can be captured.
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-SITESS_R396.");

            // Verify MS-SITESS requirement: MS-SITESS_R396
            bool isVerifyR396 = isErrorOccured;

            Site.CaptureRequirementIfIsTrue(
                isVerifyR396,
                396,
                @"[In ExportWorkflowTemplate] [workflowTemplateName:] If a workflow template with the specified name does not exist, the server will return a SOAP fault with an implementation-specific error code.");

            // If code can run to here, it means that Microsoft SharePoint Foundation 2010 and above support operation ExportWorkflowTemplate.
            this.VerifyOperationExportWorkflowTemplate();
            #endregion Capture requirements
        }

        /// <summary>
        /// This method is used to verify Microsoft SharePoint Foundation 2010 and above support operation ExportWorkflowTemplate.
        /// </summary>
        public void VerifyOperationExportWorkflowTemplate()
        {
            // If code can run to here, it means Microsoft SharePoint Foundation 2010 and above support operation ExportWorkflowTemplate.
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-SITESS_R5331, Microsoft SharePoint Foundation 2010 and above support operation ExportWorkflowTemplate.");

            // Verify MS-SITESS requirement: MS-SITESS_R5331
            Site.CaptureRequirement(
                5331,
                @"[In Appendix B: Product Behavior] Implementation does support this method [ExportWorkflowTemplate]. (Microsoft SharePoint Foundation 2010 and above follow this behavior.)");
        }

        /// <summary>
        /// Protocol server faults using SOAP faults as specified either in [SOAP1.1] section 4.4, or in [SOAP1.2/1] section 5.4.
        /// </summary>
        /// <param name="isSoapFault">Indicate whether the response is a SoapFault.</param>
        public void VerifySoapFault(bool isSoapFault)
        {
            // If an SoapException is thrown, R366 can be captured.
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-SITESS_R366, a SoapFault {0} returned.", isSoapFault ? "is" : "is not");

            // Verify MS-SITESS requirement: MS-SITESS_R366
            Site.CaptureRequirementIfIsTrue(
                isSoapFault,
                366,
                @"[In Transport] Protocol server faults can be returned using SOAP faults as specified either in [SOAP1.1] section 4.4, or in [SOAP1.2/1] section 5.4.");

            // If an SoapException is thrown, R355 can be captured.
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-SITESS_R355, a SoapFault {0} returned.", isSoapFault ? "is" : "is not");

            // Verify MS-SITESS requirement: MS-SITESS_R355
            Site.CaptureRequirementIfIsTrue(
                isSoapFault,
                355,
                @"[In Protocol Details] This protocol [MS-SITESS] allows protocol servers to notify protocol clients of application-level faults using SOAP faults.");

            // If an SoapException is thrown, R356 can be captured.
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-SITESS_R356, a SoapFault {0} returned.", isSoapFault ? "is" : "is not");

            // Verify MS-SITESS requirement: MS-SITESS_R356
            Site.CaptureRequirementIfIsTrue(
                isSoapFault,
                356,
                @"[In Protocol Details] This protocol [MS-SITESS] allows protocol servers to provide additional details for SOAP faults by including either a detail element as specified in [SOAP1.1] section 4.4, or a Detail element as specified in [SOAP1.2/1] section 5.4.5, which conforms to the XML schema of the SOAPFaultDetails complex type specified in section 2.2.4.1.");
        }

        /// <summary>
        /// Verify SoapFaultDetail message.
        /// </summary>
        /// <param name="soapFault">A SoapException contains Soap fault message.</param>
        public void VerifySoapFaultDetail(SoapException soapFault)
        {
            bool isSchemaRight = false;
            string detailBody = SchemaValidation.GetSoapFaultDetailBody(SchemaValidation.LastRawResponseXml.OuterXml);
            ValidationResult detailResult = SchemaValidation.ValidateXml(this.Site, detailBody);
            isSchemaRight = detailResult.Equals(ValidationResult.Success);

            // If the errorSchema is right, R9 can be captured.
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-SITESS_R9");

            // Verify MS-SITESS requirement: MS-SITESS_R9
            Site.CaptureRequirementIfIsTrue(
                isSchemaRight,
                9,
                @"[In SOAPFaultDetails] [The SOAPFaultDetails is defined as:] <s:schema xmlns:s=""http://www.w3.org/2001/XMLSchema"" targetNamespace="" http://schemas.microsoft.com/sharepoint/soap"">
              <s:complexType name=""SOAPFaultDetails"">
              <s:sequence>
              <s:element name=""errorstring"" type=""s:string""/>
              <s:element name=""errorcode"" type=""s:string"" minOccurs=""0""/>
              </s:sequence>
              </s:complexType>
              </s:schema>");

            string errorCode = Common.ExtractErrorCodeFromSoapFault(soapFault);

            // If error code is empty, R422 can be verified.
            if (string.IsNullOrEmpty(errorCode))
            {
                // If the errorSchema is right, R422 can be captured.
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-SITESS_R422");

                // Verify MS-SITESS requirement: MS-SITESS_R422
                Site.CaptureRequirementIfIsTrue(
                    isSchemaRight,
                    422,
                    @"[In SOAPFaultDetails] This element [errorcode] is optional.");
            }

            // If error code is not empty, R421 can be verified.
            if (!string.IsNullOrEmpty(errorCode))
            {
                // If the errorSchema is right, R421 can be captured.
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-SITESS_R421, the error code is {0}.", errorCode);

                bool isVerify421 = errorCode.Length.Equals(10) && errorCode.StartsWith("0x", StringComparison.CurrentCulture);

                // Verify MS-SITESS requirement: MS-SITESS_R421
                Site.CaptureRequirementIfIsTrue(
                    isVerify421,
                    421,
                    @"[In SOAPFaultDetails] errorcode: The hexadecimal representation of a four-byte result code.");
            }
        }

        #endregion Scenario 5 ExportWorkflowTemplate

        #endregion Test Cases

        #region Test Case Initialization & Cleanup

        /// <summary>
        /// Test Case Initialization.
        /// </summary>
        [TestInitialize]
        public void TestCaseInitialize()
        {
            this.sitessAdapter = Site.GetAdapter<IMS_SITESSAdapter>();
            Common.CheckCommonProperties(this.Site, true);
            this.sutAdapter = Site.GetAdapter<IMS_SITESSSUTControlAdapter>();
            this.solutionName = "NewSolution" + Common.FormatCurrentDateTime();
        }

        /// <summary>
        /// Test Case Cleanup.
        /// </summary>
        [TestCleanup]
        public void TestCaseCleanup()
        {
            // Remove all the solution files if they are generated successfully.
            if (this.isSolutionFileCreated)
            {
                this.sutAdapter.EmptyDocumentLibrary(string.Empty, string.Empty, Common.GetConfigurationPropertyValue(Constants.ValidLibraryName, this.Site));
                this.isSolutionFileCreated = false;
            }

            this.sitessAdapter.Reset();
            this.sutAdapter.Reset();
        }

        #endregion
    }
}