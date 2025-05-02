import { RolesEnum } from "src/app/shared/enums/roles.enum";

export const navbarItems: {
  name: string; path: string; icon: string; cssClass?: string; isLockColorWhite?: boolean;
  publicPath?: string; hideInPublic?: boolean; lockForPublic?: boolean
}[] = [
    {
      name: 'Admin',
      path: 'admin',
      icon: 'user-account',
      hideInPublic: true
    },
    {
      name: 'Manage',
      path: 'manage',
      icon: 'user-account',
      hideInPublic: true
    },
    {
      name: 'Dashboard',
      path: 'dashboard',
      icon: 'dashboard'
    },
    {
      name: 'Projects',
      path: 'projects',
      icon: 'projects',
      publicPath: 'solutions'
    },
    {
      name: 'Project Library',
      path: 'projects-library',
      icon: 'sun',
      hideInPublic: true
    },
    {
      name: 'Learn',
      path: 'learn',
      icon: 'learn'
    },
    {
      name: 'Events',
      path: 'events',
      icon: 'events'
    },
    {
      name: 'Tools',
      path: 'tools',
      icon: 'tools'
    },
    {
      name: 'Community',
      path: 'community',
      icon: 'community'
    },
    {
      name: 'Forum',
      path: 'forum',
      icon: 'forum',
      isLockColorWhite: false,
      lockForPublic: true
    },
    {
      name: 'Messages',
      path: 'messages',
      icon: 'communication-bubble',
      cssClass: 'custom-message',
      isLockColorWhite: true,
      lockForPublic: true
    }
  ];
