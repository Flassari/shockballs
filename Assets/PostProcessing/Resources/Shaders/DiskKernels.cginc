#ifndef __DISK_KERNELS__
#define __DISK_KERNELS__

#if !defined(KERNEL_SMALL) && !defined(KERNEL_MEDIUM) && \
    !defined(KERNEL_LARGE) && !defined(KERNEL_VERYLARGE)

static const int kSampleCount = 1;
static const float2 kDiskKernel[1] = { float2(0, 0) };

#endif

#if defined(KERNEL_SMALL)

// rings = 3
// points per ring = 5
static const int kSampleCount = 16;
static const float2 kDiskKernel[kSampleCount] = {
    float2(0,0),
    float2(0.375,0),
    float2(0.11588137,0.3566462),
    float2(-0.30338138,0.22041944),
    float2(-0.30338135,-0.22041951),
    float2(0.11588142,-0.35664618),
    float2(0.6875,0),
    float2(0.5561992,0.40410236),
    float2(0.21244916,0.6538514),
    float2(-0.21244921,0.65385133),
    float2(-0.55619925,0.40410233),
    float2(-0.6875,-0.00000006010316),
    float2(-0.55619913,-0.40410244),
    float2(-0.21244894,-0.6538514),
    float2(0.21244927,-0.65385133),
    float2(0.55619913,-0.40410239),
};

#endif

#if defined(KERNEL_MEDIUM)

// rings = 3
// points per ring = 7
static const int kSampleCount = 22;
static const float2 kDiskKernel[kSampleCount] = {
    float2(0,0),
    float2(0.36363637,0),
    float2(0.22672357,0.28430238),
    float2(-0.08091671,0.35451925),
    float2(-0.32762504,0.15777594),
    float2(-0.32762504,-0.15777591),
    float2(-0.08091656,-0.35451928),
    float2(0.22672352,-0.2843024),
    float2(0.6818182,0),
    float2(0.614297,0.29582983),
    float2(0.42510667,0.5330669),
    float2(0.15171885,0.6647236),
    float2(-0.15171883,0.6647236),
    float2(-0.4251068,0.53306687),
    float2(-0.614297,0.29582986),
    float2(-0.6818182,-0.00000005960644),
    float2(-0.614297,-0.29582983),
    float2(-0.42510656,-0.53306705),
    float2(-0.15171856,-0.66472363),
    float2(0.1517192,-0.6647235),
    float2(0.4251066,-0.53306705),
    float2(0.614297,-0.29582983),
};

#endif

#if defined(KERNEL_LARGE)

// rings = 4
// points per ring = 7
static const int kSampleCount = 43;
static const float2 kDiskKernel[kSampleCount] = {
    float2(0,0),
    float2(0.2758621,0),
    float2(0.1719972,0.21567768),
    float2(-0.061385095,0.26894566),
    float2(-0.24854316,0.1196921),
    float2(-0.24854316,-0.11969208),
    float2(-0.061384983,-0.2689457),
    float2(0.17199717,-0.21567771),
    float2(0.51724136,0),
    float2(0.46601835,0.22442262),
    float2(0.32249472,0.40439558),
    float2(0.11509705,0.50427306),
    float2(-0.11509704,0.50427306),
    float2(-0.3224948,0.40439552),
    float2(-0.46601835,0.22442265),
    float2(-0.51724136,-0.000000045218677),
    float2(-0.46601835,-0.22442262),
    float2(-0.32249463,-0.40439564),
    float2(-0.11509683,-0.5042731),
    float2(0.11509732,-0.504273),
    float2(0.32249466,-0.40439564),
    float2(0.46601835,-0.22442262),
    float2(0.7586207,0),
    float2(0.7249173,0.22360738),
    float2(0.6268018,0.4273463),
    float2(0.47299224,0.59311354),
    float2(0.27715522,0.7061801),
    float2(0.056691725,0.75649947),
    float2(-0.168809,0.7396005),
    float2(-0.3793104,0.65698475),
    float2(-0.55610836,0.51599306),
    float2(-0.6834936,0.32915324),
    float2(-0.7501475,0.113066405),
    float2(-0.7501475,-0.11306671),
    float2(-0.6834936,-0.32915318),
    float2(-0.5561083,-0.5159932),
    float2(-0.37931028,-0.6569848),
    float2(-0.16880904,-0.7396005),
    float2(0.056691945,-0.7564994),
    float2(0.2771556,-0.7061799),
    float2(0.47299215,-0.59311366),
    float2(0.62680185,-0.4273462),
    float2(0.72491735,-0.22360711),
};

#endif

#if defined(KERNEL_VERYLARGE)

// rings = 5
// points per ring = 7
static const int kSampleCount = 71;
static const float2 kDiskKernel[kSampleCount] = {
    float2(0,0),
    float2(0.22222224,0),
    float2(0.13855329,0.17374034),
    float2(-0.049449105,0.21665066),
    float2(-0.20021531,0.096418634),
    float2(-0.20021531,-0.09641862),
    float2(-0.049449015,-0.2166507),
    float2(0.13855328,-0.17374037),
    float2(0.41666666,0),
    float2(0.37540367,0.1807849),
    float2(0.2597874,0.3257631),
    float2(0.092717074,0.40621996),
    float2(-0.09271706,0.40621996),
    float2(-0.25978747,0.32576308),
    float2(-0.37540367,0.18078493),
    float2(-0.41666666,-0.000000036426155),
    float2(-0.37540367,-0.1807849),
    float2(-0.25978732,-0.32576317),
    float2(-0.092716895,-0.40622),
    float2(0.09271729,-0.4062199),
    float2(0.25978735,-0.32576317),
    float2(0.37540367,-0.1807849),
    float2(0.6111111,0),
    float2(0.5839611,0.18012817),
    float2(0.5049237,0.34425116),
    float2(0.38102153,0.47778592),
    float2(0.22326393,0.56886727),
    float2(0.045668334,0.60940236),
    float2(-0.13598502,0.59578925),
    float2(-0.30555558,0.52923775),
    float2(-0.4479762,0.41566107),
    float2(-0.55059206,0.26515123),
    float2(-0.60428554,0.09108128),
    float2(-0.6042855,-0.09108152),
    float2(-0.55059206,-0.26515117),
    float2(-0.4479761,-0.41566116),
    float2(-0.3055555,-0.52923775),
    float2(-0.13598506,-0.59578925),
    float2(0.045668513,-0.6094023),
    float2(0.22326423,-0.5688672),
    float2(0.38102147,-0.47778597),
    float2(0.5049237,-0.3442511),
    float2(0.5839612,-0.18012795),
    float2(0.8055556,0),
    float2(0.7853586,0.17925298),
    float2(0.7257805,0.3495175),
    float2(0.6298087,0.5022557),
    float2(0.5022557,0.6298087),
    float2(0.34951738,0.72578055),
    float2(0.17925301,0.7853586),
    float2(-0.000000035211954,0.8055556),
    float2(-0.179253,0.7853586),
    float2(-0.34951755,0.7257805),
    float2(-0.50225586,0.62980866),
    float2(-0.6298089,0.5022555),
    float2(-0.7257805,0.34951752),
    float2(-0.7853586,0.17925298),
    float2(-0.8055556,-0.00000007042391),
    float2(-0.7853586,-0.17925294),
    float2(-0.7257805,-0.3495175),
    float2(-0.62980866,-0.5022558),
    float2(-0.50225556,-0.62980884),
    float2(-0.34951726,-0.7257806),
    float2(-0.17925267,-0.7853587),
    float2(0.000000393725,-0.8055556),
    float2(0.17925343,-0.7853585),
    float2(0.34951726,-0.7257806),
    float2(0.5022556,-0.62980884),
    float2(0.62980866,-0.50225574),
    float2(0.7257805,-0.3495175),
    float2(0.7853586,-0.17925292),
};

#endif

#endif // __DISK_KERNELS__
