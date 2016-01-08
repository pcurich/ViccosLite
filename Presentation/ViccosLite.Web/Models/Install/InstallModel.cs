﻿using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ViccosLite.Framework.Mvc;
using ViccosLite.Web.Validators.Install;
using FluentValidation.Attributes;

namespace ViccosLite.Web.Models.Install
{
    [Validator(typeof(InstallValidator))]
    public class InstallModel : BaseSoftModel
    {
        [AllowHtml]
        public string AdminEmail { get; set; }

        [AllowHtml]
        [DataType(DataType.Password)]
        public string AdminPassword { get; set; }

        [AllowHtml]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [AllowHtml]
        public string DatabaseConnectionString { get; set; }

        public string DataProvider { get; set; }
        public bool DisableSqlCompact { get; set; }
        //SQL Server properties
        public string SqlConnectionInfo { get; set; }

        [AllowHtml]
        public string SqlServerName { get; set; }

        [AllowHtml]
        public string SqlDatabaseName { get; set; }

        [AllowHtml]
        public string SqlServerUsername { get; set; }

        [AllowHtml]
        public string SqlServerPassword { get; set; }

        public string SqlAuthenticationType { get; set; }
        public bool SqlServerCreateDatabase { get; set; }
        public bool UseCustomCollation { get; set; }

        [AllowHtml]
        public string Collation { get; set; }

        public bool DisableSampleDataOption { get; set; }
        public bool InstallSampleData { get; set; }
    }
}