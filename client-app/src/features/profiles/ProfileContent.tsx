import React from 'react'
import {Tab} from 'semantic-ui-react';
import ProfilePhotos from './ProfilePhotos';
import ProfileDescription from './ProfileDescription';

const panes = [
    {menuItem: 'About', render: () => <ProfileDescription />},
    {menuItem: 'Photos', render: () => <ProfilePhotos />},
    {menuItem: 'Activities', render: () => <Tab.Pane>Activities content</Tab.Pane>},
    {menuItem: 'Followers', render: () => <Tab.Pane>Followers content</Tab.Pane>},
    {menuItem: 'Following', render: () => <Tab.Pane>Following content</Tab.Pane>},
]

const ProfileContent = () => {
    return (
        <Tab 
            menu={{fluid: true, vertical: true}}
            menuPosition='right'
            panes={panes}
        />
    )
}

export default ProfileContent
