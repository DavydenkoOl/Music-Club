// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

"use strict"
let state = "play";
let player_btn = document.getElementById("player_btn");
if (player_btn != null) {
    function playPause() {
        if (state === "play") {
            playbtnImg.src = "/image/pause.png";
            state = "pause";
            video_player.play();

        }
        else {
            playbtnImg.src = "/image/play-button.png";
            state = "play"
            video_player.pause();
        }
    }
    player_btn.onclick = () => {
        playPause()
    }
    let volumeValue = 1;

    volumeRange.volume = 1;

    volumeRange.addEventListener("input", () => {
        video_player.volume = volumeRange.value / 100;
    });
    volumeRange.addEventListener("change", () => {
        video_player.volume = volumeRange.value / 100;
    });
}