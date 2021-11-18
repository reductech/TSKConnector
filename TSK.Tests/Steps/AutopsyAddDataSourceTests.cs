using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CSharpFunctionalExtensions;
using Moq;
using Reductech.EDR.Connectors.TSK.Steps;
using Reductech.EDR.Core;
using Reductech.EDR.Core.ExternalProcesses;
using Reductech.EDR.Core.Internal;
using Reductech.EDR.Core.Internal.Errors;
using Reductech.EDR.Core.TestHarness;
using Reductech.EDR.Core.Util;

namespace Reductech.EDR.Connectors.TSK.Tests.Steps
{

public partial class AutopsyAddDataSourceTests : StepTestBase<AutopsyAddDataSource, Unit>
{
    /// <inheritdoc />
    protected override IEnumerable<StepCase> StepCases
    {
        get
        {
            yield return new StepCase(
                        "Add Data Source",
                        new AutopsyAddDataSource()
                        {
                            DataSourcePath    = StaticHelpers.Constant("TestDataSourcePath"),
                            CaseDirectory     = StaticHelpers.Constant("TestCaseDirectory"),
                            IngestProfileName = StaticHelpers.Constant("TestIngestProfile"),
                        },
                        Unit.Default
                    )
                    .WithTestTSKSettings()
                    .WithExternalProcessAction(
                        x => x.Setup(
                                runner => runner
                                    .RunExternalProcess(
                                        "C:/AutopsyTest",
                                        IgnoreNoneErrorHandler.Instance,
                                        new[]
                                        {
                                            "--nosplash", "--caseDir", "TestCaseDirectory",
                                            "--addDataSource", "--dataSourcePath",
                                            "TestDataSourcePath", "--runIngest",
                                            "TestIngestProfile"
                                        },
                                        It.IsAny<IReadOnlyDictionary<string, string>>(),
                                        Encoding.UTF8,
                                        It.IsAny<IStateMonad>(),
                                        It.IsAny<IStep>(),
                                        It.IsAny<CancellationToken>()
                                    )
                            )
                            .ReturnsAsync(Result.Success<Unit, IErrorBuilder>(Unit.Default))
                    )
                ;
        }
    }

    /// <inheritdoc />
    protected override IEnumerable<ErrorCase> ErrorCases =>
        base.ErrorCases.Select(x => TestHelpers.WithTestTSKSettings<ErrorCase>(x));
}

}