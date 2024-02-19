//thêm một số hàm kiểm tra dữ liệu nhập
$(function () {
    //đây là hàm kiểm tra ngày tháng dạng ngày/tháng/năm
    $.validator.addMethod('date',
    function (value, element) {
        if (this.optional(element)) {
            return true;
        }
        var ok = true;
        try {
            $.datepicker.parseDate('dd/mm/yy', value);
        }
        catch (err) {
            ok = false;
        }
        return ok;
    }, 'Bạn phải nhập dạng ngày/tháng/năm');
    $(".datefield").datepicker({ dateFormat: 'dd/mm/yy', changeYear: true, yearRange: '1950:2030' }, $.datepicker.regional["vi"]);

    //đây là hàm kiểm tra định dạng nhập thời gian dạng giờ:phút
    $.validator.addMethod("validtime", function (value, element) {
        var arr_time = value.split(':');
        if ($.isNumeric(arr_time[0]) == false || $.isNumeric(arr_time[1]) == false) {
            return false;
        }
        if (arr_time[0] < 0 || arr_time[0] > 23) {
            return false;
        }
        if (arr_time[1] < 0 || arr_time[1] > 59) {
            return false;
        }
        return true;
    }, "Bạn phải nhập thời gian dạng giờ:phút");
    jQuery.validator.unobtrusive.adapters.add('validtime', function (options) {
        options.rules['validtime'] = {};
        options.messages['validtime'] = options.message;
    });
    //đây là hàm định nghĩa các dữ liệu nhập dạng input mask
    $(":input").inputmask();
});
//-----------------------------------------------------------------------------
//thêm tooltip cho tab
$('[data-toggle="tab"]').tooltip({
    trigger: 'hover',
    placement: 'top',
    animate: true,
    delay: 500,
    container: 'body'
});
//-----------------------------------------------------------------------------
//Preview hình ảnh mới vừa chọn xong lên trang web
//trước khi upload
//trả về bool
//input: chính là cái <input type="file" id="Avatar" name="upload" />
//imgID: id của cái tag hình ảnh sẽ được hiển thị, ví dụ <img id="hienThi" src="#" style="display:none;" width="200" />
function Thanh_readURL(input, imgID) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#'+imgID).attr('src', e.target.result);
            $('#'+imgID).show();
        }

        reader.readAsDataURL(input.files[0]);
    }
}
//check file có nằm trong danh sách các loại file được chọn hay không
//trả về bool
//input: chính là cái <input type="file" id="Avatar" name="upload" />
//arr_ext: mảng chứa các loại đuôi mở rộng cho phép, ví dụ: var ar_ext = ['png', 'gif', 'jpg']; 
function Thanh_checkFile(input, ar_ext) {
    var name = input.value;
    var ar_name = name.split('.');
    var ext = ar_name.pop(); //cái cuối cùng sau '.' mới là filetype, tránh trường hợp tên file có cả '.'

    // check the file extension
    var re = 0;
    for (var i = 0; i < ar_ext.length; i++) {
        if (ar_ext[i].toLowerCase() == ext.toLowerCase()) {
            re = 1;
            break;
        }
    }
    // if re is 1, the extension is in the allowed list
    if (re == 1) {        
        return true;
    }
    else {
        // delete the file name, disable Submit, Alert message        
        return false;
    }
}
