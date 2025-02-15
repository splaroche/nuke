// Copyright 2021 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Linq;
using Nuke.Common.Execution;
using Nuke.Common.Utilities.Collections;

namespace Nuke.Common.CI
{
    public class GenerateBuildServerConfigurationsAttribute
        : BuildServerConfigurationGenerationAttributeBase, IOnBuildCreated
    {
        public void OnBuildCreated(NukeBuild build, IReadOnlyCollection<ExecutableTarget> executableTargets)
        {
            var configurationId = EnvironmentInfo.GetParameter<string>(ConfigurationParameterName);
            if (configurationId == null)
                return;

            Assert.NotNull(NukeBuild.RootDirectory);

            var generator = GetGenerators(build)
                .Where(x => x.Id == configurationId)
                .SingleOrDefaultOrError($"Found multiple {nameof(IConfigurationGenerator)} with same ID '{configurationId}'.")
                .NotNull("generator != null");

            generator.Generate(build, executableTargets);

            Environment.Exit(0);
        }
    }
}
