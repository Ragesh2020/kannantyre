

//paste this code under the head tag or in a separate js file.
// Wait for window load
$(window).load(function () {
    // Animate loader off screen

    setTimeout(function () {
        $('.se-pre-con').fadeOut('slow', function () {
        });
    }, 1000);
    // $('.se-pre-con').fadeOut('slow');
});