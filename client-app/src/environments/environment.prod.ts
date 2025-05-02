export const environment = {
  projectName: 'Zeigo Network',
  production: true,
  disableLog: true,
  encodeUri: true,
  wordpressUrl: 'https://wp-network.zeigo.com/wp-login.php',
  apiUrl: 'https://network.zeigo.com/zeigonetwork/prod/api',
  apiKey: '',
  authorityDomain: 'subb2c.b2clogin.com',
  signUpSignIn: {
    name: 'B2C_1A_ZNSIGNIN',
    authority: 'https://subb2c.b2clogin.com/subb2c.onmicrosoft.com/B2C_1A_ZNSIGNIN'
  },
  passwordReset: {
    authority: 'https://subb2c.b2clogin.com/subb2c.onmicrosoft.com/B2C_1A_ZNPasswordReset'
  },
  changePassword: {
    authority: 'https://subb2c.b2clogin.com/subb2c.onmicrosoft.com/B2C_1A_ZNChangePassword',
    redirectUri: 'https://network.zeigo.com/zeigonetwork/settings/resetcomplete'
  },
  authClientId: '94fc12f1-f552-4e48-8c82-f05f7acd08ab',
  redirect: '/zeigonetwork/auth-redirect',
  postLogoutRedirect: '/zeigonetwork/',
  baseAppUrl: '/zeigonetwork/',
  scopes: ['https://subb2c.onmicrosoft.com/neonetwork3/neonetwork3-api-access'],
  mapBox: {
    accessToken:
      'pk.eyJ1Ijoic2NobmVpZGVyZXNzbm9ucHJvZCIsImEiOiJja2ZwczJmY2EyOTY2MnNtang4bmU0ZW11In0.76yWkJ8VDCn83AmyLsFSNg',
    styles: 'mapbox://styles/schneideressnonprod/ckfs9fgij18u219p7jow25smf',
    api: 'https://api.mapbox.com/geocoding/v5'
  },
  recaptcha: {
    siteKey: '6LdKMPgfAAAAAPoAI22i0yESA6jOkWnmbhoCfGms'
  },
  logging: {
    level: 'Critical', // TODO: change back to Error after investigation 500 errors on prod
    publishers: ['WebApi']
  },
  quoteScreenCompanies: [
    { id: 2, image: 'pivot-energy.png' },
    { id: 586, image: 'powerflex.png' },
    { id: 539, image: 'next-era.png' },
    { id: 536, image: 'edp-renewables-na.png' },
    { id: 588, image: 'sol-systems.png' }
  ]
};
