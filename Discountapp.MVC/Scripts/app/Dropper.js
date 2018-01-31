/// <reference path="../typings/jquery/jquery.d.ts" />
/// <reference path="../typings/dropzone/dropzone.d.ts" />
var Dropper = (function () {
    //private $self: JQuery;
    function Dropper(url, select, container) {
        if (container === void 0) { container = "#dropzoneContainer"; }
        this.url = url;
        this.select = select;
        this.container = container;
        this.Collection = [];
        this.template = "\n<div class=\"row\">\n    <div class=\"col-lg-12 col-md-12 col-sm-12 col-xs-12\">\n        <div class=\"dz-preview dz-file-preview\">\n            <div class=\"dz-details\">\n                <div class=\"_dz-filename\"><span _data-dz-name></span></div>\n                <div class=\"dz-size\" data-dz-size></div>\n                <img data-dz-thumbnail src=\"http://lorempixel.com/350/250/animals/2\" />\n            </div>\n            <div class=\"progress progress-small progress-striped active\">\n                <div class=\"progress-bar progress-bar-success\" data-dz-uploadprogress>\n                    <br />\n                </div>\n            </div>\n        </div>\n        <div class=\"dz-success-mark\"><span>\u2714</span></div>\n        <div class=\"dz-error-mark\"><span>\u2718</span></div>\n        <div class=\"dz-error-message\">\n            <span data-dz-errormessage></span>\n        </div>\n        <!--<span data-dz-suc class=\"text-success\">\u0418\u0437\u043E\u0431\u0440\u0430\u0436\u0435\u043D\u0438\u0435 \u0437\u0430\u0433\u0440\u0443\u0436\u0435\u043D\u043E</span>-->\n        <div>\n            <a class=\"dz-remove btn btn-danger\" href=\"javascript:undefined;\" data-dz-remove=\"\">\u0423\u0434\u0430\u043B\u0438\u0442\u044C</a>\n        </div>\n    </div>\n</div>";
    }
    Dropper.prototype.CreateDropzone = function (selector) {
        var $self = $(selector);
        $(selector).dropzone({
            url: this.url,
            paramName: "file",
            maxFilesize: 20,
            maxFiles: 1,
            dictDefaultMessage: "Добавьте одну фотографию",
            dictResponseError: 'Server not Configured',
            acceptedFiles: ".png,.jpg,.gif,.bmp,.jpeg",
            thumbnailHeight: 250,
            thumbnailWidth: 300,
            init: function () {
                var self = this;
                // config
                self.options.addRemoveLinks = false;
                self.options.dictRemoveFile = "Удалить";
                //New file added
                self.on("addedfile", function (file) {
                    console.log('new file added ');
                });
                // Send file starts
                self.on("sending", function (file) {
                    console.log('upload started');
                    $('.meter').show();
                });
                self.on("maxfilesexceeded", function (file) {
                    alert("Удалите изображение и добавьте новое");
                    self.removeFile(file);
                    //self.removeAllFiles();
                    //self.addFile(file);
                });
                // File upload Progress
                self.on("totaluploadprogress", function (progress) {
                    console.log("progress ", progress);
                    $('.roller').width(progress + '%');
                });
                self.on("queuecomplete", function (progress) {
                    $('.meter').delay(999).slideUp(999);
                });
                // On removing file
                self.on("removedfile", function (file) {
                    console.log("remove");
                });
                self.on("success", function (data, serverResult) {
                    console.log("result", data, serverResult);
                    $self.find("input[type=\"hidden\"]").val(serverResult.Name);
                    $self.find(".progress-bar-success").hide();
                });
            },
            previewTemplate: this.template
        });
    };
    Dropper.prototype.GenerateNewItem = function (callback) {
        var _this = this;
        $.getJSON("/api/Category", function (data) {
            var categoriesString = new CategoryOption(data).render();
            var dropperItem = new DropperItem(_this.Collection.length, categoriesString);
            _this.add(dropperItem);
            var itemHtml = dropperItem.Render();
            _this.setCloseEvent(itemHtml);
            $(_this.container).append(itemHtml);
            _this.CreateDropzone(itemHtml.find(".dropzone"));
            _this.hideCloseBtns();
            return dropperItem;
        });
    };
    Dropper.prototype.setCloseEvent = function (itemHtml) {
        var _this = this;
        var $close = itemHtml.find(".close");
        var $itemId = parseInt($close.attr("data-value"));
        $close.click(function () {
            _this.Collection.splice($itemId, 1);
            itemHtml.remove();
            console.log(_this.Collection);
        });
    };
    Dropper.prototype.add = function (item) {
        this.Collection.push(item);
    };
    Dropper.prototype.hideCloseBtns = function () {
        var $rows = $(".promotion-item");
        $rows.each(function (i, e) {
            var $e = $(e);
            if (i !== $rows.length - 1) {
                $e.find(".close").hide();
            }
            if (i === $rows.length - 1) {
                $e.find(".close").show();
            }
            console.log(i, $rows.length);
        });
    };
    return Dropper;
}());
var CategoryOption = (function () {
    function CategoryOption(categories) {
        this.categories = categories;
    }
    CategoryOption.prototype.render = function () {
        var template = "";
        this.categories.forEach(function (e, i) {
            template += "<option value=\"" + e.Id + "\">" + e.Value + "</option>";
        });
        return template;
    };
    return CategoryOption;
}());
var DropperItem = (function () {
    function DropperItem(index, categories) {
        this.index = index;
        this.categories = categories;
        this.template = "<section class=\"alert alert-dismissible promotion-item\">\n    <div class=\"row\">\n        <button type=\"button\" class=\"close\" data-value=" + this.index + " _data-dismiss=\"alert\" aria-label=\"_Close\"><span aria-hidden=\"true\">&times;</span></button>\n    </div>\n    <div class=\"row\">\n        <div class=\"col-lg-6 col-md-6 col-sm-12 col-xs-12\">\n            <div action=\"\" class=\"dropzone dz-clickable\">\n                <input type=\"file\" _multiple=\"\" name=\"file\" style=\"display:none;\">\n                <input type=\"hidden\" value id=\"PromotionItems_" + this.index + "__TempFolder\" name=\"PromotionItems[" + this.index + "].TempFolder\" />\n            </div>\n        </div>\n        <div class=\"col-lg-6 col-md-6 col-sm-12 col-xs-12\">\n            <div class=\"is-empty row\">\n                <div class=\"col-md-12\">\n                    <div class=\"form-group is-empty\">\n                        <label class=\"control-label\" for=\"PromotionItems_" + this.index + "__Name\">\u041D\u0430\u0437\u0432\u0430\u043D\u0438\u0435 \u0442\u043E\u0432\u0430\u0440\u0430</label>\n                        <input class=\"form-control text-box single-line\" id=\"PromotionItems_" + this.index + "__Name\" name=\"PromotionItems[" + this.index + "].Name\" placeholder=\"\u041D\u0430\u0437\u0432\u0430\u043D\u0438\u0435 \u0442\u043E\u0432\u0430\u0440\u0430 (\u043D\u0435\u043E\u0431\u044F\u0437\u0430\u0442\u0435\u043B\u044C\u043D\u043E\u0435)\" type=\"text\" value=\"\">\n                    </div>\n                    <span class=\"field-validation-valid text-danger\" data-valmsg-for=\"item.Name\" data-valmsg-replace=\"true\"></span>\n                </div>\n            </div>\n            <div class=\"is-empty row\">\n                <div class=\"col-md-12\">\n                    <div class=\"form-group is-empty\">\n                        <label class=\"control-label\" for=\"PromotionItems_" + this.index + "__CategoryId\">\u041A\u0430\u0442\u0435\u0433\u043E\u0440\u0438\u044F</label>\n                        <select class=\"form-control valid\" id=\"PromotionItems_" + this.index + "__CategoryId\" name=\"PromotionItems[" + this.index + "].CategoryId\" placeholder=\"\u041A\u0430\u0442\u0435\u0433\u043E\u0440\u0438\u044F \u0442\u043E\u0432\u0430\u0440\u0430\" aria-invalid=\"false\">\n                            <option></option>\n                            " + this.categories + "\n                        </select>\n                    </div>\n                </div>\n            </div>\n            <div class=\"is-empty row\">\n                <div class=\"col-md-12\">\n                    <div class=\"form-group is-empty\">\n                        <label class=\"control-label\" for=\"PromotionItems_" + this.index + "__BeginPrice\">\u041D\u0430\u0447\u0430\u043B\u044C\u043D\u0430\u044F \u0446\u0435\u043D\u0430</label>\n                        <input class=\"form-control text-box single-line\" id=\"PromotionItems_" + this.index + "__BeginPrice\" name=\"PromotionItems[" + this.index + "].BeginPrice\" placeholder=\"\u041D\u0430\u0447\u0430\u043B\u044C\u043D\u0430\u044F \u0446\u0435\u043D\u0430\" type=\"text\" value=\"\">\n                    </div>\n                    <span class=\"field-validation-valid text-danger\" data-valmsg-for=\"item.BeginPrice\" data-valmsg-replace=\"true\"></span>\n                </div>\n            </div>\n            <div class=\"is-empty row\">\n                <div class=\"col-md-12\">\n                    <div class=\"form-group is-empty\">\n                        <label class=\"control-label\" for=\"PromotionItems_" + this.index + "__PromotionalPrice\">\u0410\u043A\u0446\u0438\u043E\u043D\u0430\u0430\u044F \u0446\u0435\u043D\u0430</label>\n                        <input class=\"form-control text-box single-line\" id=\"PromotionItems_" + this.index + "__PromotionalPrice\" name=\"PromotionItems[" + this.index + "].PromotionalPrice\" placeholder=\"\u0410\u043A\u0446\u0438\u043E\u043D\u0430\u0430\u044F \u0446\u0435\u043D\u0430\" type=\"text\" value=\"\">\n                    </div>\n                </div>\n            </div>\n            <div class=\"is-empty row\">\n                <div class=\"col-md-12\">\n                    <div class=\"form-group is-empty\">\n                        <label class=\"control-label\" for=\"PromotionItems_" + this.index + "__Discount\">\u0421\u043A\u0438\u0434\u043A\u0430 %</label>\n                        <input class=\"form-control text-box single-line\" id=\"PromotionItems_" + this.index + "__Discount\" name=\"PromotionItems[" + this.index + "].Discount\" placeholder=\"\u0421\u043A\u0438\u0434\u043A\u0430 %\" type=\"text\" value=\"\">\n                    </div>\n                </div>\n            </div>\n        </div>\n    </div>\n</section>";
    }
    DropperItem.prototype.Render = function () {
        return $(this.template);
    };
    return DropperItem;
}());
//# sourceMappingURL=Dropper.js.map