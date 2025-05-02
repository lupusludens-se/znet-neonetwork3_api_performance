export const environment = {
  projectName: 'Zeigo Network',
  production: false,
  disableLog: false,
  encodeUri: true,
  wordpressUrl: 'https://wp-network-dev.zeigo.com/wp-login.php',
  apiUrl: 'https://network-dev.zeigo.com/zeigonetwork/dev/api',
  apiKey: '',
  authorityDomain: 'subnpb2c.b2clogin.com',
  signUpSignIn: {
    name: 'B2C_1A_ZNSIGNIN',
    authority: 'https://subnpb2c.b2clogin.com/subnpb2c.onmicrosoft.com/B2C_1A_ZNSIGNIN'
  },
  passwordReset: {
    authority: 'https://subnpb2c.b2clogin.com/subnpb2c.onmicrosoft.com/B2C_1A_ZNPasswordReset',
    redirectUri: 'https://network-dev.zeigo.com/zeigonetwork/dev/login'
  },
  changePassword: {
    authority: 'https://subnpb2c.b2clogin.com/subnpb2c.onmicrosoft.com/B2C_1A_ZNChangePassword',
    redirectUri: 'https://network-dev.zeigo.com/zeigonetwork/dev/settings/resetcomplete'
  },
  authClientId: 'be67923b-a8b2-4034-a184-cabbb7047a52',
  redirect: '/zeigonetwork/dev/auth-redirect',
  postLogoutRedirect: '/zeigonetwork/dev/',
  baseAppUrl: '/zeigonetwork/dev/',
  scopes: ['https://subnpb2c.onmicrosoft.com/neonetwork3/neonetwork3-api-access'],
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
    level: 'Trace',
    publishers: ['WebApi']
  },
  quoteScreenCompanies: [
    { id: 44, image: 'pivot-energy.png' },
    { id: 45, image: 'powerflex.png' },
    { id: 46, image: 'next-era.png' },
    { id: 47, image: 'edp-renewables-na.png' },
    { id: 48, image: 'sol-systems.png' }
  ]
};
