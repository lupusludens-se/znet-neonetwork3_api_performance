export const environment = {
  projectName: 'Zeigo Network',
  production: true,
  disableLog: true,
  encodeUri: true,
  wordpressUrl: 'https://wp-network.zeigo.com/wp-login.php',
  apiUrl: 'https://network-pre.zeigo.com/zeigonetwork/preprod/api',
  apiKey: '',
  authorityDomain: 'subpreprodb2c.b2clogin.com',
  signUpSignIn: {
    name: 'B2C_1A_ZNSIGNIN',
    authority: 'https://subpreprodb2c.b2clogin.com/subpreprodb2c.onmicrosoft.com/B2C_1A_ZNSIGNIN'
  },
  passwordReset: {
    authority: 'https://subpreprodb2c.b2clogin.com/subpreprodb2c.onmicrosoft.com/B2C_1A_ZNPasswordReset',
    redirectUri: 'https://network-pre.zeigo.com/zeigonetwork/preprod/login'
  },
  changePassword: {
    authority: 'https://subpreprodb2c.b2clogin.com/subpreprodb2c.onmicrosoft.com/B2C_1A_ZNChangePassword',
    redirectUri: 'https://network-pre.zeigo.com/zeigonetwork/preprod/settings/resetcomplete'
  },
  authClientId: '9e44cba6-0146-42bc-ade5-7b19bb44b486',
  redirect: '/zeigonetwork/preprod/auth-redirect',
  postLogoutRedirect: '/zeigonetwork/preprod/',
  baseAppUrl: '/zeigonetwork/preprod/',
  scopes: ['https://subpreprodb2c.onmicrosoft.com/neonetwork3/neonetwork3-api-access'],
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
    level: 'Warning',
    publishers: ['WebApi']
  },
  quoteScreenCompanies: [
    { id: 362, image: 'pivot-energy.png' },
    { id: 332, image: 'powerflex.png' },
    { id: 336, image: 'next-era.png' },
    { id: 364, image: 'edp-renewables-na.png' },
    { id: 365, image: 'sol-systems.png' }
  ]
};
