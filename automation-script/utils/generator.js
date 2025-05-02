// Function to generate a random string
function generateRandomString(length) {
    let result = '';
    let characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    let charactersLength = characters.length;
    for (var i = 0; i < length; i++) {
      result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }
    return result;
  }
  
  // Generate a random email ID
  function generateRandomEmail() {
    let randomString = generateRandomString(8); // Generate a random string for the email
    let email = 'Test'+randomString + '@yopmail.com'; // Combine with a domain name
    return email;
  }
  
  // Store the random email ID in a variable
  let randomEmail = generateRandomEmail();
  console.log(randomEmail); 