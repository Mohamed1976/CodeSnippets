const StorageKey = 'bookmarks';

//DOM
const sitename=document.getElementById('sitename');
const siteurl=document.getElementById('siteurl');
const bookmarkresults = document.getElementById('bookmarkresults');    

//EventListener
document.getElementById('form1').addEventListener('submit',saveBookmark);

function fetchBookmarks() {

    //localStorage.reset()
    const bookmarks = loadBookmarks();
    if(bookmarks !== null) { /* Load bookmarks if data present.  */
        console.log(bookmarks);

        bookmarkresults.innerHTML = "";                                               
        for(let i=0; i < bookmarks.length; i++) { 
            const name=bookmarks[i].name;
            const url=bookmarks[i].url;
            bookmarkresults.innerHTML+= '<div class="card bg-light text-dark card-body">'+
                                '<h3>'+name+'&nbsp'+
                                '<a class="btn btn-dark"  target="_blank"   href=" '+addhttp(url)+' "> visit</a>'+
                                ' <a onclick="deleteBookmark(\''+url+'\')" class="btn btn-danger" href="#">Delete</a> ' +
                                '</h3>'+
                                '</div>'; 
        }
    }
}

function addhttp(url) {
    if (!/^(?:f|ht)tps?\:\/\//.test(url)) {
        url = "http://" + url;
    }

    return url;
}
 
function deleteBookmark(url)
{
    let bookmarks = loadBookmarks();
    
    for(let i=0;i<bookmarks.length;i++){
        if(bookmarks[i].url === url){
            bookmarks.splice(i,1);
        }
    }

    saveBookmarks(bookmarks);
    fetchBookmarks();
}

function saveBookmark(sender) {
    sender.preventDefault();
    const name = sitename.value.trim();
    const url = siteurl.value.trim();

    if(urlIsValid(name,url)){
        const bookmark = {
            name:name,
            url:url
        }

        let bookmarks = loadBookmarks();
        if(bookmarks === null) { /* Load bookmarks if data present.  */
            bookmarks = new Array();
        }

        bookmarks.push(bookmark);
        saveBookmarks(bookmarks);

        document.getElementById('form1').reset();
        fetchBookmarks();
    }
}

function urlIsValid(sitename, siteurl){
    //https://www.w3resource.com/javascript-exercises/javascript-regexp-exercise-9.php
    const regexp =  /^(?:(?:https?|ftp):\/\/)?(?:(?!(?:10|127)(?:\.\d{1,3}){3})(?!(?:169\.254|192\.168)(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)(?:\.(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)*(?:\.(?:[a-z\u00a1-\uffff]{2,})))(?::\d{2,5})?(?:\/\S*)?$/;
    const isValid = sitename && siteurl && regexp.test(siteurl); 
    
    return isValid;    
}


function loadBookmarks() {
    let bookmarks = null;

    const json = localStorage.getItem(StorageKey);
    
    if(json !== null){ /* Load bookmarks if data present.  */
        bookmarks = JSON.parse(json);
    }

    return bookmarks;
}

function saveBookmarks(bookmarks) {
    localStorage.setItem(StorageKey,JSON.stringify(bookmarks));
}