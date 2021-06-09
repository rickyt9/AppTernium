// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let options = {
    startAngle: -1.55,
    size: 150,
    value: 1.0,
    fill: { gradient: ["#ff3300","#ff9900"] }
}
$(".circle .bar").circleProgress(options).on('circle-animation-progress',
    function (event, progress, stepValue) {

    });