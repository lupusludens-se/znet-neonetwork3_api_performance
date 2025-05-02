export const environment = {
  projectName: 'Zeigo Network',
  production: false,
  disableLog: false,
  encodeUri: true,
  apiUrl: 'https://localhost:7203/zeigonetwork/local/api',
  wordpressUrl: 'https://wp-network-dev.zeigo.com/wp-login.php',
  apiKey: '',
  authorityDomain: 'subnpb2c.b2clogin.com',
  signUpSignIn: {
    name: 'B2C_1A_ZNSIGNIN',
    authority: 'https://subnpb2c.b2clogin.com/subnpb2c.onmicrosoft.com/B2C_1A_ZNSIGNIN'
  },
  passwordReset: {
    authority: 'https://subnpb2c.b2clogin.com/subnpb2c.onmicrosoft.com/B2C_1A_ZNPasswordReset'
  },
  changePassword: {
    authority: 'https://subnpb2c.b2clogin.com/subnpb2c.onmicrosoft.com/B2C_1A_ZNChangePassword',
    redirectUri: 'http://localhost:4200/'
  },
  authClientId: 'be67923b-a8b2-4034-a184-cabbb7047a52',
  redirect: '/auth-redirect',
  postLogoutRedirect: '/',
  baseAppUrl: '/',
  scopes: ['https://subnpb2c.onmicrosoft.com/neonetwork3/neonetwork3-api-access'],
  mapBox: {
    accessToken:
      'pk.eyJ1Ijoic2NobmVpZGVyZXNzbm9ucHJvZCIsImEiOiJja2ZwczJmY2EyOTY2MnNtang4bmU0ZW11In0.76yWkJ8VDCn83AmyLsFSNg',
    styles: 'mapbox://styles/schneideressnonprod/ckfs9fgij18u219p7jow25smf',
    api: 'https://api.mapbox.com/geocoding/v5'
  },
  recaptcha: {
    siteKey: '6LeIxAcTAAAAAJcZVRqyHh71UMIEGNQ_MXjiZKhI'
  },
  logging: {
    level: 'Trace',
    publishers: ['Console']
  },
  quoteScreenCompanies: [
    { id: 1, image: 'pivot-energy.png' },
    { id: 2, image: 'powerflex.png' },
    { id: 3, image: 'next-era.png' },
    { id: 4, image: 'edp-renewables-na.png' },
    { id: 5, image: 'sol-systems.png' }
  ]
};
