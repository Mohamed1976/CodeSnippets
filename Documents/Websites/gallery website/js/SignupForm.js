window.addEventListener('DOMContentLoaded', StartApp);

function StartApp() {
    let user = []
    let form = document.getElementById('form')
    let name = document.getElementById('name')
    let email = document.getElementById('email')
    let pass = document.getElementById('pass')

    form.addEventListener('submit', (e) => {
        e.preventDefault()
        if (name.value == '' || email.value == '' || pass.value == '') {
            alertError('Empty fields')
        } else {
            let details = {}
            details.name = name.value
            details.emil = email.value
            details.pass = pass.value
            console.log(`${name.value}, ${email.value}, ${pass.value}`);

            if (pass.value.length < 6) {
                alertError('Password most be six character long')
            } else {
                user.push(details)
                console.log(user)
                alertError('Successfully Added')
                name.value = ''
                email.value = ''
                pass.value = ''
            }
            
        }
    })

    function alertError(err) {
        let alert = document.getElementById('msg')
        alert.innerHTML = `${err}`
        setTimeout(() => {
            alert.innerHTML = ''
        }, 3000)
    }
}