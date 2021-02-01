const toggle = document.getElementById("toggle");
const body = document.querySelector("body");
//const { body } = document;
toggle.addEventListener("change", event => {
  if (event.target.checked) {
    body.className = "dark";
  } else {
    body.className = "light";
  }
});