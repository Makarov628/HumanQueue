import * as React from 'react';
import ToggleButton from '@mui/material/ToggleButton';
import ToggleButtonGroup from '@mui/material/ToggleButtonGroup';

import * as api from '../../../api';

interface ILanguageItem {
    id: number,
    litera: string
    title: string
}

export interface ILanguage {
    currentLang: string
    availableLanguages: api.AvailableCultureResponse[]
    select: (lang: string) => void | null
}

export default function Language({ ... props}: ILanguage) {
    const [language, setLanguage] = React.useState(props.currentLang);
    const [items, setItems] = React.useState<api.AvailableCultureResponse[]>([]);

    React.useEffect(() => {
        setItems(props.availableLanguages)
    },[props.availableLanguages]);

    const ChangeLang = (
        event: React.MouseEvent<HTMLElement>,
        lang: string,
    ) => {
        setLanguage(lang);
        if (props.select) {
            props.select(lang)
        }
    };

    return (
        <ToggleButtonGroup
            color="primary"
            value={language}
            exclusive
            size='large'
            onChange={ChangeLang}
        >
            {
                items.map((lang: api.AvailableCultureResponse, index: number) => {
                    return (
                        <ToggleButton 
                            key={`lang_${index}`} 
                            value={lang.culture ?? ""}>
                                {lang.languageName}
                        </ToggleButton>
                    )
                })
            }
        </ToggleButtonGroup>
    );
}