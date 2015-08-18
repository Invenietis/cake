using Cake.Arguments;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Diagnostics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Code.Cake
{
    /// <summary>
    /// Crappy implementation: it is just a POC.
    /// </summary>
    public class CodeCakeApplication
    {
        IDictionary<string, Type> _builds;

        /// <summary>
        /// Runs the application.
        /// </summary>
        /// <param name="args">Arguments.</param>
        /// <returns>0 on success.</returns>
        public int Run( string[] args )
        {
            var console = new CakeConsole();
            var logger = new CakeBuildLog( console );
            var engine = new CakeEngine( logger );

            IFileSystem fileSystem = new FileSystem();
            ICakeEnvironment environment = new CakeEnvironment();
            IGlobber globber = new Globber( fileSystem, environment );
            ICakeArguments arguments = new CakeArguments();
            IProcessRunner processRunner = new ProcessRunner( environment, logger );
            IToolResolver nuGetToolResolver = new LocalNuGetToolResolver( fileSystem, globber );
            IRegistry windowsRegistry = new WindowsRegistry();

            // Parse options.
            var argumentParser = new ArgumentParser( logger, fileSystem );
            var options = argumentParser.Parse( args );
            Debug.Assert( options != null );
            logger.SetVerbosity( options.Verbosity );
            Type choosenBuild;
            if( !AvailableBuilds.TryGetValue( options.Script, out choosenBuild ) )
            {
                logger.Error( "Build script '{0}' not found.", options.Script );
                return -1;
            }

            var context = new CakeContext( fileSystem, environment, globber, logger, arguments, processRunner, new[] { nuGetToolResolver }, windowsRegistry );

            // Copy the arguments from the options.
            context.Arguments.SetArguments( options.Arguments );

            // Set the working directory: the solution directory.
            var solutionPath = new Uri( Assembly.GetEntryAssembly().CodeBase ).LocalPath;
            solutionPath = System.IO.Path.GetDirectoryName( solutionPath );
            solutionPath = System.IO.Path.GetDirectoryName( solutionPath );
            solutionPath = System.IO.Path.GetDirectoryName( solutionPath );
            solutionPath = System.IO.Path.GetDirectoryName( solutionPath );
            environment.WorkingDirectory = new DirectoryPath( solutionPath );

            CodeCakeHost._injectedActualHost = new BuildScriptHost( engine, context );
            CodeCakeHost c = (CodeCakeHost)Activator.CreateInstance( choosenBuild );

            try
            {
                var strategy = new DefaultExecutionStrategy( logger );
                var report = engine.RunTarget( context, strategy, "Default" );
                if( report != null && !report.IsEmpty )
                {
                    var printerReport = new CakeReportPrinter( console );
                    printerReport.Write( report );
                }
            }
            catch( Exception ex )
            {
                logger.Error( "Error occured: '{0}'.", ex.Message );
                return -1;
            }
            return 0;
        }

        /// <summary>
        /// Gets a mutable dictionary of build objects.
        /// </summary>
        public IDictionary<string, Type> AvailableBuilds
        {
            get
            {
                return _builds ?? (_builds = Assembly.GetEntryAssembly()
                                                        .ExportedTypes
                                                        .Where( t => t != typeof( CodeCakeHost ) && typeof( CodeCakeHost ).IsAssignableFrom( t ) )
                                                        .ToDictionary( t => t.Name ));
            }
        }

    }
}
