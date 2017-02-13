
$(document).ready(function () {
    var $CarouselSection = $('#Carousel').find('div').filter('.item');
    //set first carousel item on active
    $($CarouselSection).first().addClass('active');
});