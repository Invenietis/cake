using Cake.Core;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code.Cake
{
    class LocalNuGetToolResolver : INuGetToolResolver
    {
        private readonly IFileSystem _fileSystem;
        private readonly IGlobber _globber;
        private IFile _cachedPath;

        public LocalNuGetToolResolver( IFileSystem fileSystem, IGlobber globber )
        {
            if( fileSystem == null )
            {
                throw new ArgumentNullException( "fileSystem" );
            }
            if( globber == null )
            {
                throw new ArgumentNullException( "globber" );
            }
            _fileSystem = fileSystem;
            _globber = globber;
        }

        public string Name
        {
            get { return "NuGet"; }
        }

        public FilePath ResolveToolPath()
        {
            // Check if path allready resolved
            if( _cachedPath != null && _cachedPath.Exists )
            {
                return _cachedPath.Path;
            }

            // Check if tool exists in tool folder
            const string expression = "./packages/**/NuGet.exe";
            var toolsExe = _globber.GetFiles( expression ).FirstOrDefault();
            if( toolsExe != null )
            {
                var toolsFile = _fileSystem.GetFile( toolsExe );
                if( toolsFile.Exists )
                {
                    _cachedPath = toolsFile;
                    return _cachedPath.Path;
                }
            }

            throw new CakeException( "Could not locate nuget.exe." );
        }
    }
}
