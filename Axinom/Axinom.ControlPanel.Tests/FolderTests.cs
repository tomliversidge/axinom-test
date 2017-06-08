using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace Axinom.ControlPanel.Tests
{
    public class FolderTests
    {
        [Fact]
        public void can_convert_file_paths_to_object_hierarchy()
        {
            var folder = new Folder();
            folder.Add("foobar/");
            folder.Add("foobar/baz/");
            folder.Add("foobar/baz/Orleans-Technical-Paper.pdf");
            folder.Add("foobar/sagas.pdf");
            folder.Add("foobar/serverless-ops.epub");
            folder.Add("Get_Programming_with_FSharp_v8_MEAP.epub");
            folder.Add("Something-Else.epub");
            
            folder.Files.ShouldContain("Get_Programming_with_FSharp_v8_MEAP.epub");
            folder.Files.ShouldContain("Something-Else.epub");
            folder.Folders.ShouldContainKey("foobar");
            var foobar = folder.Folders["foobar"];
            foobar.Folders.ShouldContainKey("baz");
            foobar.Files.ShouldContain("sagas.pdf");
            foobar.Files.ShouldContain("serverless-ops.epub");
            var baz = foobar.Folders["baz"];
            baz.Files.ShouldContain("Orleans-Technical-Paper.pdf");
        }
    }
}