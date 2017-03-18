
var modalFormFromTable = (function () {
    var $btnCreate = $('.btnCreate');
    var $btnEdit = $('.btnEdit');
    var $btnDelete = $('.btnDelete');
    var createUrl = $($btnCreate).data('ajaxUrl');//'/Admin/MenuItem/CreatePartial';
    var editUrl = $($btnEdit).data('ajaxUrl');//'/Admin/MenuItem/Edit';
    var deleteUrl = $($btnDelete).data('ajaxUrl');//'/Admin/MenuItem/Delete';
    var loadingUrl = '/Admin/Home/LoadingPartial';
    var init = function (settings) {
        //objekt configuracyjny
        modalFormFromTable.config = {
            $modal: $('#Modal'),
            $table: $('#Table'),
            $btnCreate: $btnCreate,
            $btnEdit: $btnEdit,
            $btnDelete: $btnDelete
        };
        $.extend(modalFormFromTable.config, settings);
        //rejestrowanie zdarzeń
        registerEvents();
    };
    var registerEvents = function () {
        modalFormFromTable.config.$btnCreate.on('click', createItemForm);
        modalFormFromTable.config.$btnEdit.on('click', editItemForm);
        modalFormFromTable.config.$btnDelete.on('click', deleteItem);
    };
    var unRegisterEvents = function () {
        modalFormFromTable.config.$btnCreate.off('click', createItemForm);
        modalFormFromTable.config.$btnEdit.off('click', editItemForm);
        modalFormFromTable.config.$btnDelete.off('click', deleteItem);
    };
    var createItemForm = function () {
        $.ajax({
            method: 'get',
            dataType: 'html',
            url: createUrl,
            beforeSend: function () {
                modalFormFromTable.config.$modal.load(loadingUrl);
            },
            success: function (model) {
                modalFormFromTable.config.$modal.html(model);
            },
            error: function (e) {
                modalFormFromTable.config.$modal.modal('hide');
                alert('error: createItem');
            }
        });
    };
    var editItemForm = function () {
        var dataId = $(this).data('id');
        $.ajax({
            method: 'get',
            dataType: 'html',
            data: { 'id': dataId },
            url: editUrl,
            beforeSend: function () {
                modalFormFromTable.config.$modal.load(loadingUrl);
            },
            success: function (model) {
                modalFormFromTable.config.$modal.html(model);
            },
            error: function (e) {
                modalFormFromTable.config.$modal.modal('hide');
                //alert('error: editItem');
            }
        });
    };
    var deleteItem = function () {
        var dataId = $(this).data('id');
        $.ajax({
            method: 'get',
            dataType: 'html',
            data: { 'id': dataId },
            url: deleteUrl,
            beforeSend: function () {
                modalFormFromTable.config.$modal.load(loadingUrl);
            },
            success: function (model) {
                window.location.reload();
            },
            error: function (e) {
                modalFormFromTable.config.$modal.modal('hide');
                //alert('error: editItem');
            }
        });
    };
    return {
        init: init
    };
})();