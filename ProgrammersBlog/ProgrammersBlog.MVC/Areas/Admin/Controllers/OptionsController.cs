﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using NToastNotify;
using ProgrammersBlog.Business.Abstract;
using ProgrammersBlog.Core.Utilities.Helpers.Abstract;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.MVC.Areas.Admin.Models;

namespace ProgrammersBlog.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OptionsController : Controller
    {
        private readonly AboutUsPageInfo _aboutUsPageInfo;
        private readonly IWritableOptions<AboutUsPageInfo> _aboutUsWritableOptions;
        private readonly IToastNotification _toastNotification;
        private readonly WebSiteInfo _webSiteInfo;
        private readonly IWritableOptions<WebSiteInfo> _websiteInfoWriter;
        private readonly SmtpSettings _smtpSettings;
        private readonly IWritableOptions<SmtpSettings> _smtpSettingsWriter;
        private readonly ArticleRightSideBarWidgetOptions _articleRightSideBarWidgetOptions;
        private readonly IWritableOptions<ArticleRightSideBarWidgetOptions> _articleRightSideBarWidgetOptionsWriter;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public OptionsController(IOptionsSnapshot<AboutUsPageInfo> aboutUsPageInfo, IWritableOptions<AboutUsPageInfo> aboutUsWritableOptions, IToastNotification toastNotification, IOptionsSnapshot<WebSiteInfo> webSiteInfo, IWritableOptions<WebSiteInfo> websiteInfoWriter, IOptionsSnapshot<SmtpSettings> smtpSettings, IWritableOptions<SmtpSettings> smtpSettingsWriter, IOptionsSnapshot<ArticleRightSideBarWidgetOptions> articleRightSideBarWidgetOptions, IWritableOptions<ArticleRightSideBarWidgetOptions> articleRightSideBarWidgetOptionsWriter, ICategoryService categoryService, IMapper mapper)
        {
            _aboutUsWritableOptions = aboutUsWritableOptions;
            _toastNotification = toastNotification;
            _websiteInfoWriter = websiteInfoWriter;
            _smtpSettingsWriter = smtpSettingsWriter;
            _articleRightSideBarWidgetOptionsWriter = articleRightSideBarWidgetOptionsWriter;
            _mapper = mapper;
            _articleRightSideBarWidgetOptions = articleRightSideBarWidgetOptions.Value;
            _smtpSettings = smtpSettings.Value;
            _webSiteInfo = webSiteInfo.Value;
            _aboutUsPageInfo = aboutUsPageInfo.Value;
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult About()
        {
            return View(_aboutUsPageInfo);
        }
        [HttpPost]
        public IActionResult About(AboutUsPageInfo aboutUsPageInfo)
        {
            if (ModelState.IsValid)
            {
                _aboutUsWritableOptions.Update(x =>
                {
                    x.Header = aboutUsPageInfo.Header;
                    x.Content = aboutUsPageInfo.Content;
                    x.SeoAuthor = aboutUsPageInfo.SeoAuthor;
                    x.SeoDescription = aboutUsPageInfo.SeoDescription;
                    x.SeoTags = aboutUsPageInfo.SeoTags;

                });
                _toastNotification.AddSuccessToastMessage("Hakkımızda sayfa içerikleri başarıyla güncellenmiştir.", new ToastrOptions
                {
                    Title = "Başarılı İşlem"!
                });

                return View(aboutUsPageInfo);
            }
            return View(aboutUsPageInfo);
        }
        [HttpGet]
        public IActionResult GeneralSettings()
        {
            return View(_webSiteInfo);
        }
        [HttpPost]
        public IActionResult GeneralSettings(WebSiteInfo webSiteInfo)
        {
            if (ModelState.IsValid)
            {
                _websiteInfoWriter.Update(x =>
                {
                    x.Title = webSiteInfo.Title;
                    x.MenuTitle = webSiteInfo.MenuTitle;
                    x.SeoAuthor = webSiteInfo.SeoAuthor;
                    x.SeoDescription = webSiteInfo.SeoDescription;
                    x.SeoTags = webSiteInfo.SeoTags;

                });
                _toastNotification.AddSuccessToastMessage("Sitenizin genel ayarları başarıyla güncellenmiştir.", new ToastrOptions
                {
                    Title = "Başarılı İşlem"!
                });

                return View(webSiteInfo);
            }
            return View(webSiteInfo);
        }

        [HttpGet]
        public IActionResult EmailSettings()
        {
            return View(_smtpSettings);
        }
        [HttpPost]
        public IActionResult EmailSettings(SmtpSettings smtpSettings)
        {
            if (ModelState.IsValid)
            {
                _smtpSettingsWriter.Update(x =>
                {
                    x.Server = smtpSettings.Server;
                    x.Port = smtpSettings.Port;
                    x.SenderName = smtpSettings.SenderName;
                    x.SenderEmail = smtpSettings.SenderEmail;
                    x.Username = smtpSettings.Username;
                    x.Password = smtpSettings.Password;

                });
                _toastNotification.AddSuccessToastMessage("Sitenizin e-posta ayarları başarıyla güncellenmiştir.", new ToastrOptions
                {
                    Title = "Başarılı İşlem"!
                });

                return View(smtpSettings);
            }
            return View(smtpSettings);
        }

        [HttpGet]
        public async Task<IActionResult> ArticleRightSideBarWidgetSettings()
        {
            var categoriesResult = await _categoryService.GetAllByNonDeletedAndActiveAsync();
            var articleRightSideBarWidgetOptionViewModel =
                _mapper.Map<ArticleRightSideBarWidgetOptionsViewModel>(_articleRightSideBarWidgetOptions);
            articleRightSideBarWidgetOptionViewModel.Categories = categoriesResult.Data.Categories;
            return View(articleRightSideBarWidgetOptionViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> ArticleRightSideBarWidgetSettings(ArticleRightSideBarWidgetOptionsViewModel articleRightSideBarWidgetOptionsViewModel)
        {
            var categoriesResult = await _categoryService.GetAllByNonDeletedAndActiveAsync();
            articleRightSideBarWidgetOptionsViewModel.Categories = categoriesResult.Data.Categories;
            if (ModelState.IsValid)
            {
                _articleRightSideBarWidgetOptionsWriter.Update(x =>
                {
                    x.Header = articleRightSideBarWidgetOptionsViewModel.Header;
                    x.TakeSize = articleRightSideBarWidgetOptionsViewModel.TakeSize;
                    x.CategoryId = articleRightSideBarWidgetOptionsViewModel.CategoryId;
                    x.FilterBy = articleRightSideBarWidgetOptionsViewModel.FilterBy;
                    x.OrderBy = articleRightSideBarWidgetOptionsViewModel.OrderBy;
                    x.IsAscending = articleRightSideBarWidgetOptionsViewModel.IsAscending;
                    x.StartAt = articleRightSideBarWidgetOptionsViewModel.StartAt;
                    x.EndAt = articleRightSideBarWidgetOptionsViewModel.EndAt;
                    x.MaxViewCount = articleRightSideBarWidgetOptionsViewModel.MaxViewCount;
                    x.MinViewCount = articleRightSideBarWidgetOptionsViewModel.MinViewCount;
                    x.MaxCommentCount = articleRightSideBarWidgetOptionsViewModel.MaxCommentCount;
                    x.MinViewCount = articleRightSideBarWidgetOptionsViewModel.MinViewCount;

                });
                _toastNotification.AddSuccessToastMessage("Makele sayfalarınızın widget ayarları başarıyla güncellenmiştir.", new ToastrOptions
                {
                    Title = "Başarılı İşlem"!
                });

                return View(articleRightSideBarWidgetOptionsViewModel);
            }
            return View(articleRightSideBarWidgetOptionsViewModel);
        }
    }
}
