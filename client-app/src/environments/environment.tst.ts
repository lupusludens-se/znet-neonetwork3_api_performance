export const environment = {
  projectName: 'Zeigo Network',
  production: false,
  disableLog: true,
  encodeUri: true,
  apiUrl: 'https://network-tst.zeigo.com/zeigonetwork/tst/api',
  wordpressUrl: 'https://wp-network-dev.zeigo.com/wp-login.php',
  apiKey: '',
  authorityDomain: 'subtestb2c.b2clogin.com',
  signUpSignIn: {
    name: 'B2C_1A_ZNSIGNIN',
    authority: 'https://subtestb2c.b2clogin.com/subtestb2c.onmicrosoft.com/B2C_1A_ZNSIGNIN'
  },
  passwordReset: {
    authority: 'https://subtestb2c.b2clogin.com/subtestb2c.onmicrosoft.com/B2C_1A_ZNPasswordReset',
    redirectUri: 'https://network-tst.zeigo.com/zeigonetwork/tst/login'
  },
  changePassword: {
    authority: 'https://subtestb2c.b2clogin.com/subtestb2c.onmicrosoft.com/B2C_1A_ZNChangePassword',
    redirectUri: 'https://network-tst.zeigo.com/zeigonetwork/tst/settings/resetcomplete'
  },
  authClientId: 'f9d56409-96de-49b5-a4f5-0b23da320cba',
  redirect: '/zeigonetwork/tst/auth-redirect',
  postLogoutRedirect: '/zeigonetwork/tst/',
  baseAppUrl: '/zeigonetwork/tst/',
  scopes: ['https://subtestb2c.onmicrosoft.com/neonetwork3/neonetwork3-api-access'],
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
    { id: 46, image: 'pivot-energy.png' },
    { id: 47, image: 'powerflex.png' },
    { id: 48, image: 'next-era.png' },
    { id: 49, image: 'edp-renewables-na.png' },
    { id: 50, image: 'sol-systems.png' }
  ]
};
