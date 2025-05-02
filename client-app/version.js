var fs = require('fs');

fs.readFile('version.json', 'utf-8', function(err, data) {
	if (err) throw err;

	var oldVersion = +data.split('.')[1];
	var newValue = data.replace(/"[0-9]\.[0-9]?[0-9]?[0-9]\.[0-9]"/g, '"0.' + (oldVersion+1) + '.0"');

	fs.writeFile('version.json', newValue, 'utf-8', function(err) {
		if (err) throw err;
		console.log('Version updated!');
	})
})
