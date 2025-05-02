const lighthouse = require('playwright-lighthouse');
const http = require('http');

// Simple JSON-RPC style MCP server implementation for Playwright Lighthouse
const server = http.createServer((req, res) => {
  if (req.method !== 'POST') {
    res.statusCode = 405;
    res.end('Method Not Allowed');
    return;
  }

  let body = '';
  req.on('data', chunk => {
    body += chunk.toString();
  });

  req.on('end', async () => {
    let requestData;
    try {
      requestData = JSON.parse(body);
    } catch (error) {
      res.statusCode = 400;
      res.end(JSON.stringify({ error: 'Invalid JSON' }));
      return;
    }

    const { method, params, id } = requestData;

    if (method === 'runLighthouseAudit') {
      try {
        const { url, options = {}, thresholds = {} } = params;
        
        if (!url) {
          res.statusCode = 400;
          res.end(JSON.stringify({
            id,
            error: { message: 'URL is required' }
          }));
          return;
        }

        const result = await lighthouse.audit(
          url,
          options,
          thresholds
        );

        res.statusCode = 200;
        res.setHeader('Content-Type', 'application/json');
        res.end(JSON.stringify({
          id,
          result: {
            success: true,
            data: result
          }
        }));
      } catch (error) {
        console.error('Error running lighthouse audit:', error);
        res.statusCode = 500;
        res.setHeader('Content-Type', 'application/json');
        res.end(JSON.stringify({
          id,
          error: {
            message: error.message || 'Error running lighthouse audit'
          }
        }));
      }
    } else if (method === '_listFunctions') {
      // Return available functions for MCP protocol
      res.statusCode = 200;
      res.setHeader('Content-Type', 'application/json');
      res.end(JSON.stringify({
        id,
        result: [{
          name: 'runLighthouseAudit',
          description: 'Run a Lighthouse audit on a URL',
          parameters: {
            properties: {
              url: {
                type: 'string',
                description: 'URL to audit'
              },
              options: {
                type: 'object',
                description: 'Lighthouse options'
              },
              thresholds: {
                type: 'object',
                description: 'Score thresholds'
              }
            },
            required: ['url']
          }
        }]
      }));
    } else {
      res.statusCode = 404;
      res.setHeader('Content-Type', 'application/json');
      res.end(JSON.stringify({
        id,
        error: { message: `Method ${method} not found` }
      }));
    }
  });
});

const PORT = process.env.PORT || 3000;
server.listen(PORT, () => {
  console.log(`Playwright Lighthouse MCP server running on port ${PORT}`);
}); 