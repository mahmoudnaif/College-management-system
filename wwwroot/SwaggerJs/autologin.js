
myfun= function() {
    const authOpenButton = document.querySelector('button.btn.authorize.unlocked');
    authOpenButton.click();
    
    fetch('https://localhost:7015/siginin', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ email: 'mahmoud@gmail.com', password: 'stringG1' })
    })
    .then(response => response.json())
    .then(data => {
        const token = data.data.token;
        const inputElement = document.querySelector('input[aria-label="auth-bearer-value"]');
        inputElement.value = token;
    })
    .catch(error => console.error('Error fetching token:', error));
};


setTimeout(myfun, 300);