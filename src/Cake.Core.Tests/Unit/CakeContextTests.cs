﻿using Cake.Core.Tests.Fixtures;
using Xunit;

namespace Cake.Core.Tests.Unit
{
    public sealed class CakeContextTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var fixture = new CakeContextFixture();
                fixture.FileSystem = null;

                // When
                var result = Record.Exception(() => fixture.CreateContext());

                // Then
                Assert.IsArgumentNullException(result, "fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fixture = new CakeContextFixture();
                fixture.Environment = null;

                // When
                var result = Record.Exception(() => fixture.CreateContext());

                // Then
                Assert.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Globber_Is_Null()
            {
                // Given
                var fixture = new CakeContextFixture();
                fixture.Globber = null;

                // When
                var result = Record.Exception(() => fixture.CreateContext());

                // Then
                Assert.IsArgumentNullException(result, "globber");
            }

            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given
                var fixture = new CakeContextFixture();
                fixture.Log = null;

                // When
                var result = Record.Exception(() => fixture.CreateContext());

                // Then
                Assert.IsArgumentNullException(result, "log");
            }

            [Fact]
            public void Should_Throw_If_Arguments_Are_Null()
            {
                // Given
                var fixture = new CakeContextFixture();
                fixture.Arguments = null;

                // When
                var result = Record.Exception(() => fixture.CreateContext());

                // Then
                Assert.IsArgumentNullException(result, "arguments");
            }

            [Fact]
            public void Should_Throw_If_Process_Runner_Is_Null()
            {
                // Given
                var fixture = new CakeContextFixture();
                fixture.ProcessRunner = null;

                // When
                var result = Record.Exception(() => fixture.CreateContext());

                // Then
                Assert.IsArgumentNullException(result, "processRunner");
            }
        }

        public sealed class TheFileSystemProperty
        {
            [Fact]
            public void Should_Return_Provided_File_System()
            {
                // Given
                var fixture = new CakeContextFixture();
                var context = fixture.CreateContext();

                // When
                var fileSystem = context.FileSystem;

                // Then
                Assert.Same(fixture.FileSystem, fileSystem);
            }

            [Fact]
            public void Should_Return_Provided_Environment()
            {
                // Given
                var fixture = new CakeContextFixture();
                var context = fixture.CreateContext();

                // When
                var environment = context.Environment;

                // Then
                Assert.Same(fixture.Environment, environment);
            }

            [Fact]
            public void Should_Return_Provided_Globber()
            {
                // Given
                var fixture = new CakeContextFixture();
                var context = fixture.CreateContext();

                // When
                var globber = context.Globber;

                // Then
                Assert.Same(fixture.Globber, globber);
            }

            [Fact]
            public void Should_Return_Provided_Log()
            {
                // Given
                var fixture = new CakeContextFixture();
                var context = fixture.CreateContext();

                // When
                var log = context.Log;

                // Then
                Assert.Same(fixture.Log, log);
            }

            [Fact]
            public void Should_Return_Provided_Arguments()
            {
                // Given
                var fixture = new CakeContextFixture();
                var context = fixture.CreateContext();

                // When
                var arguments = context.Arguments;

                // Then
                Assert.Same(fixture.Arguments, arguments);
            }

            [Fact]
            public void Should_Return_Provided_Process_Runner()
            {
                // Given
                var fixture = new CakeContextFixture();
                var context = fixture.CreateContext();

                // When
                var processRunner = context.ProcessRunner;

                // Then
                Assert.Same(fixture.ProcessRunner, processRunner);
            }
        }
    }
}
