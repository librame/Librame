﻿using Librame.Data;
using Librame.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Librame.UnitTests.Data
{
    [TestClass()]
    public class DataAdapterTests
    {
        [TestMethod()]
        public void DataToolsTest1()
        {
            var repository = LibrameArchitecture.AdapterManager.DataAdapter.GetCurrentRepository<Entities.Prefecture>();
            var jsonPath = @"D:\Sources\Architecture\Librame\trunk\doc\gis_prefecture_china.js";

            // 开始导入
            //Tools.ImportFacility.Import(repository, jsonPath);

            // Get
            var item = repository.Get(1);
            Assert.IsNotNull(item);
        }


        [TestMethod()]
        public void DataRepositoryTest1()
        {
            var repository = LibrameArchitecture.AdapterManager.DataAdapter.GetCurrentRepository<Entities.Region>();

            // Get
            var item = repository.Get(1);
            Assert.IsNotNull(item);

            // GetMany
            var items = repository.GetMany();
            Assert.IsNotNull(items);

            // GetPagingByIndex
            var paging = repository.GetPagingByIndex(index: 1, size: 10,
                order: (q) => q.Desc(k => k.Id), predicate: p => p.DataStatus == DataStatus.Public);
            Assert.IsNotNull(paging);
        }


        [TestMethod()]
        public void DataRepositoryTest()
        {
            var repository = LibrameArchitecture.AdapterManager.DataAdapter.GetCurrentRepository<Entities.Article>();

            // Create
            //var article = Initializer.Initialize<Entities.Article>();
            //var article = Entities.Article.Initialize();
            //var article = new Entities.Article();
            //article.Name = "Article Name";
            //article.Descr = "Article Descr";

            //for (var i = 80; i < 100; i++)
            //{
            //    var article = new Entities.Article()
            //    {
            //        Name = "Article Name " + i,
            //        Descr = "Article Descr " + i
            //    };

            //    article = repository.Create(article, false);
            //}

            // Get
            var item = repository.Get(1);
            Assert.IsNotNull(item);

            // GetMany
            var items = repository.GetMany();
            Assert.IsNotNull(items);

            // GetPagingByIndex
            var paging = repository.GetPagingByIndex(index: 1, size: 10, order: (q) => q.Desc(k => k.Id));
            Assert.IsNotNull(paging);

            // GetPagingBySkip
            paging = repository.GetPagingBySkip(skip: 20, take: 10, order: (q) => q.Asc(k => k.Id));
            Assert.IsNotNull(paging);

            // Update
            var newName = "Update " + item.Name;
            item.Name = newName;

            item = repository.Update(item);
            Assert.IsTrue(item.Name == newName);

            // Restore
            item.Name = item.Name.Replace("Update ", string.Empty);

            item = repository.Update(item);
            Assert.IsTrue(item.Name != newName);

            // Delete
            //item = repository.Get(100);
            //item = repository.Delete(item);

            var ids = repository.GetProperties(s => s.Id, p => p.Id < 20);
            Assert.IsNotNull(ids);
        }

    }
}