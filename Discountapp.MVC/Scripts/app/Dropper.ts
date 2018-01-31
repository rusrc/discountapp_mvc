/// <reference path="../typings/jquery/jquery.d.ts" />
/// <reference path="../typings/dropzone/dropzone.d.ts" />

interface ICategory {
    Id: number;
    Value: string;
}

interface IServerResult {
    IsError: boolean;
    Name: number;
    Message: number;
}

class Dropper {

    //private $self: JQuery;

    public constructor(private url: string, private select: string, private container = "#dropzoneContainer") {

    }
    public Collection: DropperItem[] = [];

    public template: string = `
<div class="row">
    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
        <div class="dz-preview dz-file-preview">
            <div class="dz-details">
                <div class="_dz-filename"><span _data-dz-name></span></div>
                <div class="dz-size" data-dz-size></div>
                <img data-dz-thumbnail src="http://lorempixel.com/350/250/animals/2" />
            </div>
            <div class="progress progress-small progress-striped active">
                <div class="progress-bar progress-bar-success" data-dz-uploadprogress>
                    <br />
                </div>
            </div>
        </div>
        <div class="dz-success-mark"><span>✔</span></div>
        <div class="dz-error-mark"><span>✘</span></div>
        <div class="dz-error-message">
            <span data-dz-errormessage></span>
        </div>
        <!--<span data-dz-suc class="text-success">Изображение загружено</span>-->
        <div>
            <a class="dz-remove btn btn-danger" href="javascript:undefined;" data-dz-remove="">Удалить</a>
        </div>
    </div>
</div>`;

    public CreateDropzone(selector: string): void;
    public CreateDropzone(selector: any): void;
    public CreateDropzone(selector: any): void {

        var $self = $(selector);

        $(selector).dropzone({
            url: this.url,
            paramName: "file", // The name that will be used to transfer the file
            maxFilesize: 20, //mb
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
                self.on("addedfile", file => {
                    console.log('new file added ');
                });
                // Send file starts
                self.on("sending", file => {
                    console.log('upload started');
                    $('.meter').show();
                });
                self.on("maxfilesexceeded", file => {
                    alert("Удалите изображение и добавьте новое");
                    self.removeFile(file);
                    //self.removeAllFiles();
                    //self.addFile(file);
                });
                // File upload Progress
                self.on("totaluploadprogress", progress => {
                    console.log("progress ", progress);
                    $('.roller').width(progress + '%');
                });

                self.on("queuecomplete", progress => {
                    $('.meter').delay(999).slideUp(999);
                });

                // On removing file
                self.on("removedfile", file => {
                    console.log("remove");
                });

                self.on("success", (data, serverResult: IServerResult) => {
                    console.log("result", data, serverResult);
                    $self.find("input[type=\"hidden\"]").val(serverResult.Name);
                    $self.find(".progress-bar-success").hide();
                });
            },
            previewTemplate: this.template
        });
    }

    public GenerateNewItem(callback: () => DropperItem) {

        $.getJSON("/api/Category",
            (data: ICategory[]) => {
                var categoriesString = new CategoryOption(data).render();
                var dropperItem = new DropperItem(this.Collection.length, categoriesString);

                this.add(dropperItem);

                var itemHtml: JQuery = dropperItem.Render();
                this.setCloseEvent(itemHtml);

                $(this.container).append(itemHtml);
                this.CreateDropzone(itemHtml.find(".dropzone"));

                this.hideCloseBtns();

                return dropperItem;
            });

       
    }

    private setCloseEvent(itemHtml: JQuery): void {

        var $close = itemHtml.find(".close");
        var $itemId = parseInt($close.attr("data-value"));
        $close.click(() => {
            this.Collection.splice($itemId, 1);
            itemHtml.remove();
            console.log(this.Collection);
        });
    }

    private add(item: DropperItem): void {
        this.Collection.push(item);
    }

    private hideCloseBtns(): void {
        var $rows = $(".promotion-item");
        $rows.each((i, e) => {
            var $e = $(e);
            if (i !== $rows.length - 1) {
                $e.find(".close").hide();
            }
            if (i === $rows.length - 1) {
                $e.find(".close").show();
            }
            console.log(i, $rows.length);
        });
    }
}

class CategoryOption {

    public constructor(private  categories: ICategory[]) {
        
    }

    public render() : string {
        var template : string = "";

        this.categories.forEach((e, i) => {
            template += `<option value="${e.Id}">${e.Value}</option>`;
        });

        return template;
    }
}

class DropperItem {
    constructor(private index: number, private categories: string) {
    }

    public template: string =
`<section class="alert alert-dismissible promotion-item">
    <div class="row">
        <button type="button" class="close" data-value=${this.index} _data-dismiss="alert" aria-label="_Close"><span aria-hidden="true">&times;</span></button>
    </div>
    <div class="row">
        <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
            <div action="" class="dropzone dz-clickable">
                <input type="file" _multiple="" name="file" style="display:none;">
                <input type="hidden" value id="PromotionItems_${this.index}__TempFolder" name="PromotionItems[${this.index}].TempFolder" />
            </div>
        </div>
        <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
            <div class="is-empty row">
                <div class="col-md-12">
                    <div class="form-group is-empty">
                        <label class="control-label" for="PromotionItems_${this.index}__Name">Название товара</label>
                        <input class="form-control text-box single-line" id="PromotionItems_${this.index}__Name" name="PromotionItems[${this.index}].Name" placeholder="Название товара (необязательное)" type="text" value="">
                    </div>
                    <span class="field-validation-valid text-danger" data-valmsg-for="item.Name" data-valmsg-replace="true"></span>
                </div>
            </div>
            <div class="is-empty row">
                <div class="col-md-12">
                    <div class="form-group is-empty">
                        <label class="control-label" for="PromotionItems_${this.index}__CategoryId">Категория</label>
                        <select class="form-control valid" id="PromotionItems_${this.index}__CategoryId" name="PromotionItems[${this.index}].CategoryId" placeholder="Категория товара" aria-invalid="false">
                            <option></option>
                            ${this.categories}
                        </select>
                    </div>
                </div>
            </div>
            <div class="is-empty row">
                <div class="col-md-12">
                    <div class="form-group is-empty">
                        <label class="control-label" for="PromotionItems_${this.index}__BeginPrice">Начальная цена</label>
                        <input class="form-control text-box single-line" id="PromotionItems_${this.index}__BeginPrice" name="PromotionItems[${this.index}].BeginPrice" placeholder="Начальная цена" type="text" value="">
                    </div>
                    <span class="field-validation-valid text-danger" data-valmsg-for="item.BeginPrice" data-valmsg-replace="true"></span>
                </div>
            </div>
            <div class="is-empty row">
                <div class="col-md-12">
                    <div class="form-group is-empty">
                        <label class="control-label" for="PromotionItems_${this.index}__PromotionalPrice">Акционаая цена</label>
                        <input class="form-control text-box single-line" id="PromotionItems_${this.index}__PromotionalPrice" name="PromotionItems[${this.index}].PromotionalPrice" placeholder="Акционаая цена" type="text" value="">
                    </div>
                </div>
            </div>
            <div class="is-empty row">
                <div class="col-md-12">
                    <div class="form-group is-empty">
                        <label class="control-label" for="PromotionItems_${this.index}__Discount">Скидка %</label>
                        <input class="form-control text-box single-line" id="PromotionItems_${this.index}__Discount" name="PromotionItems[${this.index}].Discount" placeholder="Скидка %" type="text" value="">
                    </div>
                </div>
            </div>
        </div>
    </div>
</section>`;


    public Render(): JQuery {
        return $(this.template);
    }
}