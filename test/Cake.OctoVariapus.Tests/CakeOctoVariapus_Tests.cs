﻿using System;
using System.Collections.Generic;

using Cake.Core;
using Cake.Core.IO;

using FakeItEasy;

using HttpMock;

using Xunit;

namespace Cake.OctoVariapus.Tests
{
    public class CakeOctoVariapus_Tests
    {
        [Fact]
        public void import_with_handwritten_list_should_work()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var octoVaribleImportAlias = new CakeOctoVariableImportAliasFixture(0);
            const string octopusUrl = "http://localhost";
            const string octoProjectName = "Cake.OctoVariapus";
            const string octoApiKey = "API-FZNNNTXZK0NWFHLLMYJL4JGFIU";

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            //HttpMockRepository.At("http://localhost/api/variables/variableset-Projects-1");
            octoVaribleImportAlias.CakeContext.ImportVariables(octopusUrl,
                octoProjectName,
                octoApiKey,
                new List<OctoVariable>
                {
                    new OctoVariable
                    {
                        Name = "ConnectionString",
                        IsSensitive = false,
                        IsEditable = true,
                        Value = "DataSource:localhost25",
                        Scope = new OctoScope
                        {
                            Name = "Environment",
                            Values = new List<string> { "Development", "Stage" }
                        }
                    }
                });

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
        }

        [Fact]
        public void import_from_a_json_file_should_work()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var octoVaribleImportAlias = new CakeOctoVariableImportAliasFixture(0);
            const string octopusUrl = "http://localhost";
            const string octoProjectName = "Cake.OctoVariapus";
            const string octoApiKey = "API-FZNNNTXZK0NWFHLLMYJL4JGFIU";
            //HttpMockRepository.At("http://localhost/api/variables/variableset-Projects-1");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------

            octoVaribleImportAlias.CakeContext.ImportVariables(octopusUrl,
                octoProjectName,
                octoApiKey,
                "variables.json");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
        }

        private class CakeOctoVariableImportAliasFixture
        {
            public CakeOctoVariableImportAliasFixture(int exitCode)
            {
                GetDirectoryPath = Guid.NewGuid().ToString();

                A.CallTo(() => CakeContext.ProcessRunner).Returns(ProcessRunner);
                A.CallTo(() => CakeContext.FileSystem).Returns(FileSystem);
                A.CallTo(() => CakeContext.Log).Returns(GetCakeLog);
                A.CallTo(() => ProcessRunner.Start(A<FilePath>._, A<ProcessSettings>._)).Returns(Process);
                A.CallTo(() => Process.GetExitCode()).Returns(exitCode);
            }

            public CakeOctoVariableImportAliasFixture()
                : this(0)
            {
            }

            public ICakeContext CakeContext { get; } = A.Fake<ICakeContext>();

            public IFileSystem FileSystem { get; } = A.Fake<IFileSystem>();

            public DirectoryPath GetDirectoryPath { get; }

            public IProcess Process { get; } = A.Fake<IProcess>();

            public IProcessRunner ProcessRunner { get; } = A.Fake<IProcessRunner>();

            public CakeLogFixture GetCakeLog { get; } = new CakeLogFixture();
        }
    }
}
