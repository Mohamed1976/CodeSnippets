window.addEventListener("load", function() {
    let counter = 0;

    document.getElementById("startBtn").addEventListener("click", () => {
        const progressBar = document.getElementById("progressBar");
        progressBar.value += 10; 
        document.getElementById("progressBarValue").innerText = progressBar.value;        
    });

    document.getElementById("ResetBtn").addEventListener("click", () => {
        const progressBar = document.getElementById("progressBar");
        progressBar.value = 0; 
        document.getElementById("progressBarValue").innerText = progressBar.value;        
    });
});