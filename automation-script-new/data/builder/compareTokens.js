const readline = require('readline');
/**
 * cd C:\Users\SESA751855\Documents\GitHub\znet-neonetwork3\automation-script-new\api-tests\
 * node compareTokens.js
 * Compares two JWT tokens for identity. 
 * 
 * @param {string} token1 - The first JWT token.
 * @param {string} token2 - The second JWT token.
 * @returns {boolean} - Returns true if both tokens are identical, false otherwise.
 */
function compareTokens(token1, token2) {
    // Check if both tokens are provided
    if (!token1 || !token2) {
        throw new Error("Both token1 and token2 must be provided.");
    }
    // Compare the tokens and return the result
    return token1 === token2;
}

// Create an interface to read input from the command line
const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

// Ask for the first token
rl.question('Enter the first JWT token: ', (tokenA) => {
    // Ask for the second token
    rl.question('Enter the second JWT token: ', (tokenB) => {
        // Compare the tokens
        const areTokensIdentical = compareTokens(tokenA, tokenB);
        console.log("Are the tokens identical?", areTokensIdentical);

        // Close the readline interface
        rl.close();
    });
});