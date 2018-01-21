using System;
using System.Collections.Generic;
using System.Linq;
using FileSystemVisitorLibrary.Data;
using FileSystemVisitorLibrary.Test.MockObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileSystemVisitorLibrary.Test
{
    [TestClass]
    public class FileSystemVisitor2Tests
    {
        [TestMethod]
        public void GetFileSystemEnumerableWithOutFilterTest()
        {
            // arrange 
            var provider = new MockFileSystemProvider();
            var visitor = new FileSystemVisitor(provider, "d:");

            // act  
            var result = visitor.ToList();

            // assert  
            Assert.AreEqual(result.Count, 5);
            Assert.AreEqual(result[0].Name, "FileName1");
            Assert.AreEqual(result[1].Name, "DirectoryName1");
            Assert.AreEqual(result[2].Name, "FileName2");
        }

        [TestMethod]
        public void GetFileSystemEnumerableWithFilterTest()
        {
            // arrange 
            var provider = new MockFileSystemProvider();
            var visitor = new FileSystemVisitor(
                provider,
                "d:",
                x => x.Date == new DateTime(2017, 1, 1) && x.Type == FileSystemItemType.File && x.Name.Contains("2"));

            // act  
            var result = visitor.ToList();

            // assert  
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(result[0].Name, "FileName2");
        }

        [TestMethod]
        public void GetFileSystemEnumerableEventFileFoundRaisedTest()
        {
            // arrange 
            var provider = new MockFileSystemProvider();
            var visitor = new FileSystemVisitor(provider, "d:");
            List<string> receivedEvents = new List<string>();
            visitor.FileFound += (o, eventArgs) => receivedEvents.Add(eventArgs.FoundItem.Name);

            // act  
            var result = visitor.ToList();

            // assert  
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(3, receivedEvents.Count);
            Assert.AreEqual("FileName1", receivedEvents[0]);
            Assert.AreEqual("FileName2", receivedEvents[1]);
            Assert.AreEqual("FileName3", receivedEvents[2]);
        }

        [TestMethod]
        public void GetFileSystemEnumerableEventDirectoryFoundRaisedTest()
        {
            // arrange 
            var provider = new MockFileSystemProvider();
            var visitor = new FileSystemVisitor(provider, "d:");
            List<string> receivedEvents = new List<string>();
            visitor.DirectoryFound += (o, eventArgs) => receivedEvents.Add(eventArgs.FoundItem.Name);

            // act  
            var result = visitor.ToList();

            // assert  
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(2, receivedEvents.Count);
            Assert.AreEqual("DirectoryName1", receivedEvents[0]);
            Assert.AreEqual("DirectoryName2", receivedEvents[1]);
        }

        [TestMethod]
        public void GetFileSystemEnumerableEventFilteredFileFoundRaisedTest()
        {
            // arrange 
            var provider = new MockFileSystemProvider();
            var visitor = new FileSystemVisitor(provider, "d:", x => x.Date == new DateTime(2017, 1, 1));
            List<string> receivedEvents = new List<string>();
            visitor.FilteredFileFound += (o, eventArgs) => receivedEvents.Add(eventArgs.FoundItem.Name);

            // act  
            var result = visitor.ToList();

            // assert  
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(2, receivedEvents.Count);
            Assert.AreEqual("FileName1", receivedEvents[0]);
            Assert.AreEqual("FileName2", receivedEvents[1]);
        }

        [TestMethod]
        public void GetFileSystemEnumerableEventFilteredDirectoryFoundRaisedTest()
        {
            // arrange 
            var provider = new MockFileSystemProvider();
            var visitor = new FileSystemVisitor(provider, "d:", x => x.Date == new DateTime(2017, 1, 1));
            List<string> receivedEvents = new List<string>();
            visitor.FilteredDirectoryFound += (o, eventArgs) => receivedEvents.Add(eventArgs.FoundItem.Name);

            // act  
            var result = visitor.ToList();

            // assert  
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(1, receivedEvents.Count);
            Assert.AreEqual("DirectoryName2", receivedEvents[0]);
        }

        [TestMethod]
        public void GetFileSystemEnumerableEventStartRaisedTest()
        {
            // arrange 
            var provider = new MockFileSystemProvider();
            var visitor = new FileSystemVisitor(provider, "d:");
            List<string> receivedEvents = new List<string>();
            visitor.Start += (o, eventArgs) => receivedEvents.Add("Start");

            // act  
            var result = visitor.ToList();

            // assert  
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(1, receivedEvents.Count);
            Assert.AreEqual("Start", receivedEvents[0]);
        }

        [TestMethod]
        public void GetFileSystemEnumerableEventFinishRaisedTest()
        {
            // arrange 
            var provider = new MockFileSystemProvider();
            var visitor = new FileSystemVisitor(provider, "d:");
            List<string> receivedEvents = new List<string>();
            visitor.Finish += (o, eventArgs) => receivedEvents.Add("Finish");

            // act  
            var result = visitor.ToList();

            // assert  
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(1, receivedEvents.Count);
            Assert.AreEqual("Finish", receivedEvents[0]);
        }

        [TestMethod]
        public void GetFileSystemEnumerableSkipOnFilteredFileFoundTest()
        {
            // arrange 
            var provider = new MockFileSystemProvider();
            var visitor = new FileSystemVisitor(provider, "d:");
            visitor.FilteredFileFound += (o, eventArgs) => eventArgs.IsSkip = eventArgs.FoundItem.Date == new DateTime(2017, 1, 1);

            // act  
            var result = visitor.ToList();

            // assert  
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(result[0].Name, "DirectoryName1");
            Assert.AreEqual(result[1].Name, "FileName3");
            Assert.AreEqual(result[2].Name, "DirectoryName2");
        }

        [TestMethod]
        public void GetFileSystemEnumerableSkipOnFilteredDirectoryFoundTest()
        {
            // arrange 
            var provider = new MockFileSystemProvider();
            var visitor = new FileSystemVisitor(provider, "d:");
            visitor.FilteredDirectoryFound += (o, eventArgs) => eventArgs.IsSkip = eventArgs.FoundItem.Date == new DateTime(2017, 1, 1);

            // act  
            var result = visitor.ToList();

            // assert  
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(result[0].Name, "FileName1");
            Assert.AreEqual(result[1].Name, "DirectoryName1");
            Assert.AreEqual(result[2].Name, "FileName2");
            Assert.AreEqual(result[3].Name, "FileName3");
        }

        [TestMethod]
        public void GetFileSystemEnumerableStopOnFilteredFileFoundTest()
        {
            // arrange 
            var provider = new MockFileSystemProvider();
            var visitor = new FileSystemVisitor(provider, "d:");
            visitor.FilteredFileFound += (o, eventArgs) => eventArgs.IsStop = eventArgs.FoundItem.Date == new DateTime(2017, 1, 1)
                                                                              && eventArgs.FoundItem.Name.Contains("2");

            // act  
            var result = visitor.ToList();

            // assert  
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(result[0].Name, "FileName1");
            Assert.AreEqual(result[1].Name, "DirectoryName1");
        }

        [TestMethod]
        public void GetFileSystemEnumerableStopOnFilteredDirectoryFoundTest()
        {
            // arrange 
            var provider = new MockFileSystemProvider();
            var visitor = new FileSystemVisitor(provider, "d:");
            visitor.FilteredDirectoryFound += (o, eventArgs) => eventArgs.IsStop = eventArgs.FoundItem.Date == new DateTime(2017, 1, 1)
                                                                                   && eventArgs.FoundItem.Type == FileSystemItemType.Directory;

            // act  
            var result = visitor.ToList();

            // assert  
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(result[0].Name, "FileName1");
            Assert.AreEqual(result[1].Name, "DirectoryName1");
            Assert.AreEqual(result[2].Name, "FileName2");
            Assert.AreEqual(result[3].Name, "FileName3");
        }
    }
}
