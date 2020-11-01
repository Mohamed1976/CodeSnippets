const images = document.querySelectorAll(".container img");
const previewImage = document.querySelector(".preview img");

for(const img of images){
  console.log("Adding images To addEventListener: " + img);
  img.addEventListener("click", function(e){
    console.log("Event handler on click: " + e.target.src);
    previewImage.src = e.target.src;
    previewImage.parentElement.parentElement.style.visibility = "visible";
    previewImage.parentElement.parentElement.style.opacity = 1;
    document.body.style.overflowY = "hidden";
  }, false);
}

function hidePreview(target){
  console.log("hidePreview called: " + target.style.visibility + ", " + target.style.opacity);
  target.style.visibility = "hidden";
  target.style.opacity = 0;
  previewImage.removeAttribute("src");
  document.body.style.overflowY = "auto";
}
