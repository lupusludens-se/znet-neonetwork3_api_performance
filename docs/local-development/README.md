# Local Development Setup

The following document describes requirements to develop locally.  These steps are required to access the development and test environment endpoints as well as access the database environments.

## External Development

For developers that are not leveraging the internal Schneider Electric network you are required to access the systems over VPN.  Follow the instructions below to ensure OpenVPN is setup correctly.

## Host File Updates

To properly resolve DNS to our test environments and database environments an update to your local hosts file is required.  This is due to DNS Security policy and Azure limitations in private link.

Add the following records to your host file.

```

```

* __Windows Location__:
* __Mac Location__:
* __Linux Location__:

