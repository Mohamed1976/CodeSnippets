onmessage = function(e) {
    console.log('Worker: Message received from main script');
    console.log(e);
}

var i = 0;

function timedCount() {
  i = i + 1;
  postMessage(i);
  setTimeout("timedCount()",500);
}

timedCount();